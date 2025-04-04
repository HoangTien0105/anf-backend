using ANF.Core;
using ANF.Core.Enums;
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
                    await PublishData();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error occurred while checking for spam IPs", e.StackTrace);
                    //throw;
                }
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        /// <summary>
        /// Publish data to RabbitMQ
        /// </summary>
        /// <returns></returns>
        private async Task PublishData()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var rabbitMQPublisher = scope.ServiceProvider.GetRequiredService<RabbitMQPublisher>();

                // Batch time window (getting data in a time range)
                var fromTime = DateTime.Now.AddMinutes(-6);
                var toTime = DateTime.Now;

                var trackingValidationRepository = unitOfWork.GetRepository<TrackingValidation>();
                var trackingEventRepository = unitOfWork.GetRepository<TrackingEvent>();
                var offerRepository = unitOfWork.GetRepository<Offer>();

                // Get valid tracking data based on pricing models
                // The response model to store the data from queue is ConversionResponse.cs
                // The file can change the name to a suitable one with the context

                var query = from tv in trackingValidationRepository
                                .GetAll()
                                .AsNoTracking()
                            join te in trackingEventRepository
                                .GetAll()
                                .AsNoTracking()
                            on tv.ClickId equals te.Id
                            join o in offerRepository
                                .GetAll()
                                .AsNoTracking()
                            on te.OfferId equals o.Id
                            where te.Status == TrackingEventStatus.Valid &&
                                (o.PricingModel == "CPC" || o.PricingModel == "CPA" || o.PricingModel == "CPS") &&
                                (tv.ValidatedTime >= fromTime && tv.ValidatedTime <= toTime)
                            select new
                            {
                                tv.Id,
                                tv.ClickId,
                                te.PublisherCode,
                                te.OfferId,
                                o.PricingModel,
                                tv.Amount
                            };

                var data = await query.ToListAsync();
                
                foreach (var item in data)
                {
                    // Use the pricing model to set the queue name
                    string queueName = item.PricingModel.ToLower(); // CPC, CPA, CPS
                    await rabbitMQPublisher.PublishAsync(queueName, item);

                    _logger.LogInformation($"Published event for click_id: {item.ClickId} to queue: {queueName}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                throw;
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
