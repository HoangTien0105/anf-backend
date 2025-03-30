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
            }
            await Task.Delay(_checkInterval, stoppingToken);
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
                .Where(e => e.ClickTime > tenMinutesAgo && e.ClickTime <= DateTime.Now)
                .ToListAsync();

            if (!trackingData.Any())
                throw new NoDataRetrievalException("No tracking data found for the last 10 minutes");

            var spamIps = trackingData
                .GroupBy(e => e.IpAddress)
                .Where(g => g.Count() > 5)
                .Select(g => g.Key)
                .ToList();

            if (spamIps.Any())
            {
                // Update records with spam IPs to mark them as fraud
                var fraudData = trackingData
                    .Where(e => spamIps.Contains(e.IpAddress))
                    .ToList();

                foreach (var item in fraudData)
                {
                    item.Status = Core.Enums.TrackingEventStatus.Fraud;
                    trackingEventRepository.Update(item);

                    var fraudDetection = new FraudDetection
                    {
                        ClickId = item.Id,
                        Reason = string.Empty,  // Add a short reason for the fraud detection
                        DetectedTime = DateTime.Now,
                    };
                    fraudDetectionRepository.Add(fraudDetection);
                }
                await unitOfWork.SaveAsync();

                _logger.LogInformation("Updated {count} tracking events as fraud", fraudData.Count);
            }
            else
            {
                foreach (var item in trackingData)
                {
                    item.Status = Core.Enums.TrackingEventStatus.Valid;
                    trackingEventRepository.Update(item);

                    var validation = new TrackingValidation
                    {
                        ClickId = item.Id,
                        ValidatedTime = DateTime.Now,
                        // Conversion status and revenue will be set later...
                    };
                    await unitOfWork.SaveAsync();
                }
                _logger.LogInformation("Updated {count} tracking events as valid", trackingData.Count);
            }
        }
    }
}
