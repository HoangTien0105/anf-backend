using ANF.Core;
using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ANF.Service.Backgrounds
{
    public class PublisherStatsBackgroundService(IServiceScopeFactory scopeFactory,
        ILogger<StatisticBackgroundService> logger) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<StatisticBackgroundService> _logger = logger;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"================ Stats generation started at {DateTime.Now}================");

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    await GeneratePublisherOfferStats(unitOfWork);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating advertiser stats.");
                }

                // 00:01 next day
                var delay = CalculateDelay();
                _logger.LogInformation($"Next stats generation scheduled at {DateTime.Now.Add(delay):yyyy-MM-dd HH:mm:ss zzz}");

                try
                {
                    await Task.Delay(delay, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Stats generation stopped due to cancellation.");
                    break;
                }
            }
        }

        private async Task<bool> GeneratePublisherOfferStats(IUnitOfWork unitOfWork)
        {
            try
            {
                var publisherOfferStatsRepository = unitOfWork.GetRepository<PublisherCampaignStats>();
                var userRepository = unitOfWork.GetRepository<User>();
                var publisherOfferRepository = unitOfWork.GetRepository<PublisherOffer>();
                var offerRepository = unitOfWork.GetRepository<Offer>();

                //get publishers List
                var publishers = await userRepository.GetAll()
                                          .Where(u => (u.Role == Core.Enums.UserRoles.Publisher)
                                                       && (u.Status == Core.Enums.UserStatus.Active))
                                          .ToListAsync();
                if (publishers is null || publishers.Count == 0) throw new KeyNotFoundException("Not found any active publisher");

                bool hasChanges = false;

                foreach (var publisher in publishers)
                {
                    var publisherCode = publisher.UserCode;
                    var publisherOffers = await publisherOfferRepository.GetAll()
                                                            .Where(po => ((po.PublisherCode == publisherCode)
                                                                          && (po.Status == Core.Enums.PublisherOfferStatus.Approved)))
                                                            .ToListAsync();

                    var offerIds = publisherOffers.Select(po => po.OfferId).ToList();
                    var campaigns = await offerRepository.GetAll()
                                          .Where(o => (offerIds.Contains(o.Id)
                                                       && (o.Status == Core.Enums.OfferStatus.Approved
                                                           || o.Status == Core.Enums.OfferStatus.Started)))
                                          .GroupBy(o => o.CampaignId)
                                          .ToListAsync();
                    if (campaigns is null) continue;


                    foreach (var campaign in campaigns)
                    {
                        var campaignId = campaign.Key;
                        //Chuyển các offer đã group ra thành 1 list lại
                        var campaignOffers = campaign.ToList();


                        var publisherOfferStats = await publisherOfferStatsRepository.GetAll()
                                                                                 .FirstOrDefaultAsync(s => s.CampaignId == campaign.Key);

                        /// Chạy data campaign lại 1 lần nữa
                        var newCampaignStats = await AnalyzePublisherOfferStats(campaignOffers, publisherCode, unitOfWork);
                        if (newCampaignStats is not null)
                        {
                            publisherOfferStatsRepository.Add(newCampaignStats);
                            hasChanges = true;
                        }
                    }
                }

                if (hasChanges)
                {
                    return await unitOfWork.SaveAsync() > 0;
                }
                else
                {
                    _logger.LogInformation("No new stats to save.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new ArgumentException(ex.Message);
            }
        }

        private async Task<PublisherCampaignStats?> AnalyzePublisherOfferStats(List<Offer> offers, string publisherCode, IUnitOfWork unitOfWork)
        {
            try
            {
                var trackingRepository = unitOfWork.GetRepository<TrackingEvent>();
                var validationRepository = unitOfWork.GetRepository<TrackingValidation>();
                var fraudRepository = unitOfWork.GetRepository<FraudDetection>();

                //Lấy của ngày hqua vì background chạy vào 00:01 mỗi ngày
                var yesterday = DateTime.Today.AddDays(-1);
                var endOfYesterday = yesterday.AddDays(1).AddTicks(-1);

                //1 list bao gồm các Id của Offer
                var offerIds = offers.Select(o => o.Id).ToList();

                var clickList = await trackingRepository.GetAll()
                                            .AsNoTracking()
                                            .Where(t => offerIds.Contains(t.OfferId)
                                                     && t.PublisherCode == publisherCode
                                                     && t.ClickTime >= yesterday
                                                     && t.ClickTime <= endOfYesterday)
                                            .ToListAsync();
                //Total click
                int totalClicks = clickList.Count();

                if (totalClicks == 0)
                {
                    _logger.LogInformation($"No new clicks or publishers for campaign {offers[0].CampaignId} on {yesterday:yyyy-MM-dd}.");
                    return null;
                }

                var clickIds = clickList.Select(c => c.Id)
                                        .Where(id => id != null)
                                        .ToList();

                var validatedClickList = await validationRepository.GetAll()
                                                       .AsNoTracking()
                                                       .Where(v => v.ClickId != null
                                                                    && clickIds.Contains(v.ClickId)
                                                                    && v.ConversionStatus == Core.Enums.ConversionStatus.Success
                                                                    && v.ValidatedTime >= yesterday
                                                                    && v.ValidatedTime <= endOfYesterday)
                                                       .ToListAsync();
                //Total validate clicks
                int totalValidateClicks = validatedClickList.Count();

                var totalFraudClicks = await fraudRepository.GetAll()
                                                      .AsNoTracking()
                                                      .Where(f => f.ClickId != null
                                                                    && clickIds.Contains(f.ClickId)
                                                                    && f.DetectedTime >= yesterday
                                                                    && f.DetectedTime <= endOfYesterday)
                                                      .CountAsync();

                decimal totalRevenue = 0;
                var validateClickByOffer = validatedClickList
                    .GroupBy(v => clickList.First(c => c.Id == v.ClickId).OfferId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var offer in offers)
                {
                    if (validateClickByOffer.TryGetValue(offer.Id, out var offerValidatedClicks))
                    {
                        if (offer.PricingModel == "CPS")
                        {
                            decimal sumAmount = offerValidatedClicks.Sum(v => v.Amount ?? 0);
                            totalRevenue += sumAmount * ((decimal)(offer.CommissionRate ?? 0) / 100);
                        }
                        else
                        {
                            totalRevenue += offerValidatedClicks.Count * offer.Bid;
                        }
                    }
                }

                int totalMobile = 0;
                int totalTablet = 0;
                int totalDesktop = 0;

                foreach (var click in clickList)
                {
                    if (!string.IsNullOrEmpty(click.UserAgent))
                    {
                        string lowerUa = click.UserAgent.ToLower();
                        if (lowerUa.Contains("mobile") || lowerUa.Contains("android") || lowerUa.Contains("iphone"))
                        {
                            totalMobile++;
                        }
                        else if (lowerUa.Contains("tablet") || lowerUa.Contains("ipad"))
                        {
                            totalTablet++;
                        }
                        else
                        {
                            totalDesktop++;
                        }
                    }
                }

                PublisherCampaignStats newPublisherOfferStats = new PublisherCampaignStats()
                {
                    Date = DateTime.Now,
                    CampaignId = offers.First().CampaignId,
                    TotalClick = totalClicks,
                    TotalVerifiedClick = totalValidateClicks,
                    TotalFraudClick = totalFraudClicks,
                    TotalRevenue = totalRevenue,
                    TotalMobile = totalMobile,
                    TotalTablet = totalTablet,
                    TotalComputer = totalDesktop,
                    PublisherCode = publisherCode,
                };

                return newPublisherOfferStats;
            }
            catch
            {
                return null;
            }
        }
        private TimeSpan CalculateDelay()
        {
            var now = DateTime.Now;

            var nextRun = new DateTime(now.Year, now.Month, now.Day, 0, 1, 0, DateTimeKind.Local).AddDays(1);

            if (now > nextRun)
            {
                nextRun = nextRun.AddDays(1);
            }

            return nextRun - now;
        }
    }
}
