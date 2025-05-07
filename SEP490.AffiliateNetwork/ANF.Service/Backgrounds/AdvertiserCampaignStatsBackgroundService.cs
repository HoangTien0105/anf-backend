using ANF.Core;
using ANF.Core.Enums;
using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ANF.Service.Backgrounds
{
    public class AdvertiserCampaignStatsBackgroundService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<AdvertiserCampaignStatsBackgroundService> logger) : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly ILogger<AdvertiserCampaignStatsBackgroundService> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // ORIGINAL TIMING: Runs daily at 00:01
                    // Calculate the delay until the next 00:01
                    //var now = DateTime.Now;
                    //var nextRunTime = DateTime.Today.AddDays(1).AddMinutes(1);

                    // If the current time is already past 00:01, schedule for the next day
                    //if (now > nextRunTime)
                    //{
                    //    nextRunTime = nextRunTime.AddDays(1);
                    //}

                    //var delay = nextRunTime - now;

                    // TESTING: Run after 2 minutes
                    var delay = TimeSpan.FromMinutes(2);

                    // Wait until the next run time or until the task is canceled
                    await Task.Delay(delay, stoppingToken);

                    using var scope = _serviceScopeFactory.CreateScope();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    await AddStatsDataForAdvertiser(unitOfWork);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, e.StackTrace);
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }

        /// <summary>
        /// Update advertiser's statistics data by day
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        private async Task AddStatsDataForAdvertiser(IUnitOfWork unitOfWork)
        {
            var yesterday = DateTime.UtcNow.Date.AddDays(-1);
            var startTime = yesterday;
            var endTime = yesterday.AddDays(1).AddSeconds(-1); // 23:59:59 of yesterday

            var trackingEventRepo = unitOfWork.GetRepository<TrackingEvent>();
            var campaignRepo = unitOfWork.GetRepository<Campaign>();
            var publisherOfferRepo = unitOfWork.GetRepository<PublisherOffer>();
            var advertiserCampaignStatsRepo = unitOfWork.GetRepository<AdvertiserCampaignStats>();

            // Initialize device type counters
            int totalMobile = 0, totalComputer = 0, totalTablet = 0;

            // TODO: Có cần lấy dữ liệu thống kê cho các campaign có state là "Verified" không?
            var campaigns = await campaignRepo.GetAll()
                .AsNoTracking()
                .Include(c => c.Offers)
                .Where(c => c.Status == CampaignStatus.Started)
                .ToListAsync();

            foreach (var campaign in campaigns)
            {
                var offerIds = campaign.Offers.Select(o => o.Id).ToList();
                var trackingEvents = await trackingEventRepo.GetAll()
                    .AsNoTracking()
                    .Include(te => te.TrackingValidation)
                    .Where(te => te.ClickTime >= startTime && te.ClickTime <= endTime
                        && offerIds.Contains(te.OfferId))
                    .ToListAsync();

                var totalClick = trackingEvents.Count();

                // Verified Clicks
                var totalVerifiedClick = trackingEvents
                    .Where(te => te.Status == TrackingEventStatus.Valid &&
                        te.TrackingValidation.ValidationStatus == ValidationStatus.Success)
                    .Count();

                // Fraud Clicks
                var totalFraudClick = trackingEvents
                    .Where(te => te.Status == TrackingEventStatus.Fraud)
                    .Count();

                // Thống kê gom lại của các ngày trước đó cho đến thời điểm hiện tại
                var totalJoinedPublisher = await publisherOfferRepo.GetAll()
                    .AsNoTracking()
                    .Where(po => po.Offer.CampaignId == campaign.Id
                        && po.Status == PublisherOfferStatus.Approved)
                    .Select(po => po.PublisherCode)
                    .Distinct()
                    .CountAsync();

                // Thống kê gom lại của các ngày trước đó cho đến thời điểm hiện tại
                var totalRejectedPublisher = await publisherOfferRepo.GetAll()
                    .AsNoTracking()
                    .Where(po => po.Offer.CampaignId == campaign.Id
                        && po.Status == PublisherOfferStatus.Rejected)
                    .Select(po => po.PublisherCode)
                    .Distinct()
                    .CountAsync();

                // Budget đã chi của campaign
                var spendingBudget = campaign.Budget - campaign.Balance;

                // DeviceDetectorNET
                foreach (var item in trackingEvents)
                {
                    var dd = new DeviceDetectorNET.DeviceDetector(item.UserAgent);
                    dd.Parse();

                    if (dd.IsMobile()) totalMobile++;
                    else if (dd.IsTablet()) totalTablet++;
                    else totalComputer++;
                }
                var advertiserCampaignStats = new AdvertiserCampaignStats
                {
                    CampaignId = campaign.Id,
                    Date = endTime, // Operation time
                    TotalClick = totalClick,
                    TotalVerifiedClick = totalVerifiedClick,
                    TotalFraudClick = totalFraudClick,
                    TotalJoinedPublisher = totalJoinedPublisher,
                    TotalRejectedPublisher = totalRejectedPublisher,
                    TotalBudgetSpent = (decimal)spendingBudget,
                    TotalMobile = totalMobile,
                    TotalComputer = totalComputer,
                    TotalTablet = totalTablet
                };
                advertiserCampaignStatsRepo.Add(advertiserCampaignStats);
            }
            await unitOfWork.SaveAsync();
        }
    }
}
