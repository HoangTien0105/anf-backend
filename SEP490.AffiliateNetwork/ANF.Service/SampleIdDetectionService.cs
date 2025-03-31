using ANF.Core;
using ANF.Core.Exceptions;
using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ANF.Service
{
    public sealed class SampleIdDetectionService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<SampleIdDetectionService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5);

        public SampleIdDetectionService(IServiceScopeFactory serviceScopeFactory,
            ILogger<SampleIdDetectionService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Spam IP Detection Service started at: {time}", DateTime.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckForSpamIps();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error occurred while checking for spam IPs", e.StackTrace);
                    //throw;
                }
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        private async Task CheckForSpamIps()
        {
            _logger.LogInformation("Starting spam IP check at: {time}", DateTime.Now);

            // Create a new scope because of the lifetime of background service 
            // and Unit of Work: Singleton vs. Scoped
            using var scope = _serviceScopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var trackingEventRepository = unitOfWork.GetRepository<TrackingEvent>();
            var trackingValidationRepository = unitOfWork.GetRepository<TrackingValidation>();
            var fraudDetectionRepository = unitOfWork.GetRepository<FraudDetection>();

            // Get data from the last 10 minutes
            var tenMinutesAgo = DateTime.Now.AddMinutes(-10);
            var trackingData = await trackingEventRepository
                .GetAll()
                .Where(e => e.ClickTime > tenMinutesAgo && e.ClickTime <= DateTime.Now && e.Status == Core.Enums.TrackingEventStatus.Pending)
                .ToListAsync();

            if (!trackingData.Any())
                throw new NoDataRetrievalException("No tracking data found for the last 10 minutes");

            var spamIps = trackingData
                .GroupBy(e => e.IpAddress)
                .Where(g => g.Count() > 5)
                .Select(g => g.Key)
                .ToList();

            var fraudEvents = new List<TrackingEvent>();    // For logging purpose
            var validEvents = new List<TrackingEvent>();    // For logging purpose

            foreach (var trackingItem in trackingData)
            {
                if (spamIps.Contains(trackingItem.IpAddress))
                {
                    trackingItem.Status = Core.Enums.TrackingEventStatus.Fraud;
                    fraudEvents.Add(trackingItem);
                    var fraudDetection = new FraudDetection
                    {
                        ClickId = trackingItem.Id,
                        Reason = $"Detect duplicated IP address: {trackingItem.IpAddress}",
                        DetectedTime = DateTime.Now,
                    };
                    fraudDetectionRepository.Add(fraudDetection);
                }
                else
                {
                    trackingItem.Status = Core.Enums.TrackingEventStatus.Valid;
                    validEvents.Add(trackingItem);
                    trackingValidationRepository.Add(new TrackingValidation
                    {
                        ClickId = trackingItem.Id,
                        ValidatedTime = DateTime.Now,
                    });
                }
                trackingEventRepository.Update(trackingItem);
            }

            await unitOfWork.SaveAsync();

            if (fraudEvents.Any())
                _logger.LogInformation("Updated {count} tracking events as fraud", fraudEvents.Count);
            if (validEvents.Any())
                _logger.LogInformation("Updated {count} tracking events as valid", validEvents.Count);

        }
    }
}
