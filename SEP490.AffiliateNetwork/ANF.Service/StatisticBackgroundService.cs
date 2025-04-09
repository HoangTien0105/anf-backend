using ANF.Core;
using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ANF.Service
{
    public class StatisticBackgroundService(IServiceScopeFactory scopeFactory,
        ILogger<AdvertiserStatsAggregatorService> logger) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<AdvertiserStatsAggregatorService> _logger = logger;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var runTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 0);
                if (now > runTime)
                    runTime = runTime.AddDays(1);

                var delay = runTime - now;
                _logger.LogInformation($"Next stats generation scheduled at {runTime} UTC");

                await Task.Delay(delay, stoppingToken);

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    await generatePublisherOfferStats(unitOfWork);
                    await genrateAdvertiserOfferStats(unitOfWork);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating advertiser stats.");
                }
            }
        }
        private async Task<bool> genrateAdvertiserOfferStats(IUnitOfWork unitOfWork)
        {
            try
            {
                var advertiserOfferStatsRepo = unitOfWork.GetRepository<AdvertiserOfferStats>();
                var userRepo = unitOfWork.GetRepository<User>();
                var campaignRepo = unitOfWork.GetRepository<Campaign>();
                var OfferRepo = unitOfWork.GetRepository<Offer>();

                //get advertisers List
                var advertisers = userRepo.GetAll()
                                          .Where(u => (u.Role == Core.Enums.UserRoles.Advertiser)
                                                       && (u.Status == Core.Enums.UserStatus.Active));
                if (advertisers == null) throw new KeyNotFoundException("Not found any active advertiser");

                //get campaigns list
                var advertiserCodes = advertisers.Select(a => a.UserCode).ToList();
                var campaigns = campaignRepo.GetAll()
                                            .Where(c => (advertiserCodes.Contains(c.AdvertiserCode)
                                                         && (c.Status == Core.Enums.CampaignStatus.Started
                                                            || c.Status == Core.Enums.CampaignStatus.Verified)))
                                            .ToList();
                if (campaigns == null) throw new KeyNotFoundException("Don't have either Started or Verified campaign");

                //get offers list
                var campaignIds = campaigns.Select(c => c.Id).ToList();
                var offers = OfferRepo.GetAll()
                                      .Where(o => (campaignIds.Contains(o.CampaignId)
                                                   && (o.Status == Core.Enums.OfferStatus.Approved
                                                       || o.Status == Core.Enums.OfferStatus.Started)))
                                      .ToList();
                if (offers == null) throw new KeyNotFoundException("Don't have either Approved or Started offer");

                //analyse
                foreach (var o in offers)
                {
                    var advertiserOfferStats = advertiserOfferStatsRepo.GetAll()
                                                                             .FirstOrDefault(s => s.OfferId == o.Id);

                    if (advertiserOfferStats != null)
                    {
                        var newAdvertiserOfferStats = analyzeAdvertiserOfferStats(o, unitOfWork) ?? throw new ArgumentException("Error in analyze process");
                        newAdvertiserOfferStats.Id = advertiserOfferStats.Id;
                        advertiserOfferStatsRepo.Update(newAdvertiserOfferStats);
                    }
                    else
                    {
                        var newAdvertiserOfferStats = analyzeAdvertiserOfferStats(o, unitOfWork) ?? throw new ArgumentException("Error in analyze process");
                        advertiserOfferStatsRepo.Add(newAdvertiserOfferStats);
                    }
                }

                return await unitOfWork.SaveAsync() > 0;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new ArgumentException(ex.Message);
            }
        }

        private async Task<bool> generatePublisherOfferStats(IUnitOfWork unitOfWork)
        {
            try
            {
                var publisherOfferStatsRepo = unitOfWork.GetRepository<PublisherOfferStats>();
                var userRepo = unitOfWork.GetRepository<User>();
                var publisherOfferRepo = unitOfWork.GetRepository<PublisherOffer>();
                var OfferRepo = unitOfWork.GetRepository<Offer>();

                //get publishers List
                var publishers = userRepo.GetAll()
                                          .Where(u => (u.Role == Core.Enums.UserRoles.Publisher)
                                                       && (u.Status == Core.Enums.UserStatus.Active));
                if (publishers == null) throw new KeyNotFoundException("Not found any active publisher");

                foreach (var publisher in publishers)
                {
                    var publisherCode = publisher.UserCode;
                    var publisherOffers = publisherOfferRepo.GetAll()
                                                            .Where(po => ((po.PublisherCode == publisherCode)
                                                                          && (po.Status == Core.Enums.PublisherOfferStatus.Approved)))
                                                            .ToList();
                    var offerIds = publisherOffers.Select(po => po.OfferId).ToList();
                    var offers = OfferRepo.GetAll()
                                          .Where(o => (offerIds.Contains(o.CampaignId)
                                                       && (o.Status == Core.Enums.OfferStatus.Approved
                                                           || o.Status == Core.Enums.OfferStatus.Started)))
                                          .ToList();
                    if (offers == null) throw new KeyNotFoundException("Don't have either Approved or Started offer");

                    //analyse
                    foreach (var o in offers)
                    {
                        var publisherOfferStats = publisherOfferStatsRepo.GetAll()
                                                                                 .FirstOrDefault(s => s.OfferId == o.Id);

                        if (publisherOfferStats != null)
                        {
                            var newPublisherOfferStats = analyzePublisherOfferStats(o, publisherCode, unitOfWork)
                                                          ?? throw new ArgumentException("Error in analyze process");
                            newPublisherOfferStats.Id = publisherOfferStats.Id;
                            publisherOfferStatsRepo.Update(newPublisherOfferStats);
                        }
                        else
                        {
                            var newPublisherOfferStats = analyzePublisherOfferStats(o, publisherCode, unitOfWork)
                                                          ?? throw new ArgumentException("Error in analyze process");
                            publisherOfferStatsRepo.Add(newPublisherOfferStats);
                        }
                    }
                }

                return await unitOfWork.SaveAsync() > 0;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new ArgumentException(ex.Message);
            }
        }


        private PublisherOfferStats? analyzePublisherOfferStats(Offer offer, string publisherCode, IUnitOfWork unitOfWork)
        {
            try
            {
                var trackingRepo = unitOfWork.GetRepository<TrackingEvent>();
                var validationRepo = unitOfWork.GetRepository<TrackingValidation>();
                var fraudRepo = unitOfWork.GetRepository<FraudDetection>();

                var clickList = trackingRepo.GetAll()
                                            .Where(t => t.OfferId == offer.Id)
                                            .ToList();
                var clickIds = clickList.Select(c => c.Id)
                                        .Where(id => id != null)
                                        .ToList();
                int clicksCount = clickList.Count();

                var fraudClickList = fraudRepo.GetAll().ToList();
                var fraudClickIds = fraudRepo.GetAll()
                                             .Where(f => f.ClickId != null && clickIds.Contains(f.ClickId))
                                             .Select(f => f.ClickId)
                                             .ToList();

                var validatedClickList = validationRepo.GetAll()
                                                       .AsNoTracking()
                                                       .Where(v => v.ClickId != null
                                                                    && clickIds.Contains(v.ClickId)
                                                                    && !fraudClickIds.Contains(v.ClickId)
                                                                    && v.ValidationStatus == Core.Enums.ValidationStatus.Success)
                                                       .ToList();

                int validatedClicksCount = validatedClickList.Count();

                decimal revenue = (offer.PricingModel == "CPS")
                                  ? (validatedClickList.Sum(v => v.Amount ?? 0) * (decimal)(offer.CommissionRate ?? 0))
                                  : (validatedClicksCount * offer.Bid);

                PublisherOfferStats newPublisherOfferStats = new PublisherOfferStats()
                {
                    Date = DateTime.UtcNow,
                    OfferId = offer.Id,
                    PublisherCode = publisherCode,
                    ClickCount = clicksCount,
                    ConversionCount = validatedClicksCount,
                    ConversionRate = clicksCount == 0
                                         ? 0
                                         : Math.Round((decimal)validatedClicksCount / clicksCount, 2),
                };

                return newPublisherOfferStats;
            }
            catch
            {
                return null;
            }
        }
        private AdvertiserOfferStats? analyzeAdvertiserOfferStats(Offer offer, IUnitOfWork unitOfWork)
        {
            try
            {
                var trackingRepo = unitOfWork.GetRepository<TrackingEvent>();
                var validationRepo = unitOfWork.GetRepository<TrackingValidation>();
                var fraudRepo = unitOfWork.GetRepository<FraudDetection>();
                var publisherOfferRepo = unitOfWork.GetRepository<PublisherOffer>();

                var clickList = trackingRepo.GetAll()
                                            .Where(t => t.OfferId == offer.Id)
                                            .ToList();
                var clickIds = clickList.Select(c => c.Id)
                                        .Where(id => id != null)
                                        .ToList();
                int clicksCount = clickList.Count();

                var fraudClickList = fraudRepo.GetAll().ToList();
                var fraudClickIds = fraudRepo.GetAll()
                                             .Where(f => f.ClickId != null && clickIds.Contains(f.ClickId))
                                             .Select(f => f.ClickId)
                                             .ToList();

                var validatedClickList = validationRepo.GetAll()
                                                       .AsNoTracking()
                                                       .Where(v => v.ClickId != null
                                                                    && clickIds.Contains(v.ClickId)
                                                                    && !fraudClickIds.Contains(v.ClickId)
                                                                    && v.ValidationStatus == Core.Enums.ValidationStatus.Success)
                                                       .ToList();

                int validatedClicksCount = validatedClickList.Count();

                var publisherOfferList = publisherOfferRepo.GetAll()
                                                           .Where(po => po.OfferId == offer.Id)
                                                           .ToList();
                int publisherCount = publisherOfferList.Count();

                decimal revenue = (offer.PricingModel == "CPS")
                                  ? (validatedClickList.Sum(v => v.Amount ?? 0) * (decimal)(offer.CommissionRate ?? 0))
                                  : (validatedClicksCount * offer.Bid);

                AdvertiserOfferStats newAdvertiserOfferStats = new AdvertiserOfferStats()
                {
                    Date = DateTime.UtcNow,
                    OfferId = offer.Id,
                    ClickCount = clicksCount,
                    ConversionCount = validatedClicksCount,
                    ConversionRate = clicksCount == 0
                                         ? 0
                                         : Math.Round((decimal)validatedClicksCount / clicksCount, 2),
                    PublisherCount = publisherCount,
                    Revenue = revenue
                };

                return newAdvertiserOfferStats;
            }
            catch
            {
                return null;
            }
        }
    }
}
