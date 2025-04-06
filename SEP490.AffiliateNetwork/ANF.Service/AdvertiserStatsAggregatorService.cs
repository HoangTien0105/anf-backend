using ANF.Core;
using ANF.Core.Enums;
using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ANF.Service
{
    public sealed class AdvertiserStatsAggregatorService(IServiceScopeFactory scopeFactory,
        ILogger<AdvertiserStatsAggregatorService> logger) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<AdvertiserStatsAggregatorService> _logger = logger;

        // TODO: Background service sẽ chạy vào cuối mỗi ngày, tổng hợp dữ liệu
        // và insert vô advertiser_stats
        // Điều chỉnh thời gian và check lại workflow
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var runTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 0); // 23:59 UTC
                if (now > runTime)
                    runTime = runTime.AddDays(1);

                var delay = runTime - now;
                _logger.LogInformation($"Next stats generation scheduled at {runTime} UTC");

                await Task.Delay(delay, stoppingToken);

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    await GenerateStatsAsync(unitOfWork);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating advertiser stats.");
                }
            }
        }

        private async Task GenerateStatsAsync(IUnitOfWork unitOfWork)
        {
            /*try
            {
                var targetedDate = DateTime.UtcNow.Date.AddDays(-1);

                var offerRepository = unitOfWork.GetRepository<Offer>();
                var campaignRepository = unitOfWork.GetRepository<Campaign>();
                var trackingEventRepository = unitOfWork.GetRepository<TrackingEvent>();
                var fraudRepository = unitOfWork.GetRepository<FraudDetection>();
                var trackingValidationRepository = unitOfWork.GetRepository<TrackingValidation>();
                var postbackRepository = unitOfWork.GetRepository<PostbackData>();
                var advertiserStatsRepository = unitOfWork.GetRepository<AdvertiserOfferStats>();

                // TODO: Check whether the click time in TrackingEvent is in UTC format?
                var stats = await (from o in offerRepository.GetAll().AsNoTracking()
                                   join c in campaignRepository.GetAll().AsNoTracking() on o.CampaignId equals c.Id
                                   join te in trackingEventRepository.GetAll().AsNoTracking() on o.Id equals te.OfferId
                                   where te.ClickTime.ToUniversalTime() == targetedDate
                                   group te by new { o.Id, o.CampaignId } into g
                                   select new AdvertiserOfferStats
                                   {
                                       OfferId = g.Key.Id,
                                       CampaignId = g.Key.CampaignId,
                                       Date = targetedDate,
                                       ClickCount = g.Count(),
                                       ConversionCount = trackingValidationRepository.GetAll()
                                       .AsNoTracking()
                                       .Count(tv =>
                                           g.Select(x => x.Id).Contains(tv.ClickId)
                                       ),
                                       PostbackSuccess = postbackRepository.GetAll()
                                       .AsNoTracking()
                                       .Count(pd =>
                                           g.Select(x => x.Id).Contains(pd.ClickId.ToString()) && pd.Status == PostbackStatus.Success
                                       ),
                                       PostbackFailed = postbackRepository.GetAll()
                                       .AsNoTracking()
                                       .Count(pd =>
                                           g.Select(x => x.Id).Contains(pd.ClickId.ToString()) && pd.Status == PostbackStatus.Failed
                                       ),
                                       ValidationSuccess = trackingValidationRepository.GetAll()
                                       .AsNoTracking()
                                       .Count(tv =>
                                           g.Select(x => x.Id).Contains(tv.ClickId)
                                       ),
                                       ValidationFailed = fraudRepository.GetAll()
                                       .AsNoTracking()
                                       .Count(tv =>
                                           g.Select(x => x.Id).Contains(tv.ClickId)
                                       ),
                                       ConversionRate = 0,
                                       CreatedAt = DateTime.UtcNow,
                                       UpdatedAt = DateTime.UtcNow
                                   }).ToListAsync();

                foreach (var s in stats)
                {
                    s.ConversionRate = s.ClickCount == 0 ? 0 : (decimal)s.ConversionCount / s.ClickCount;
                }

                advertiserStatsRepository.AddRange(stats);
                await unitOfWork.SaveAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                throw;
            }*/
            throw new NotImplementedException();
        }
    }
}
