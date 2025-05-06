using ANF.Core;
using ANF.Core.Commons;
using ANF.Core.Enums;
using ANF.Core.Exceptions;
using ANF.Core.Models.Entities;
using ANF.Service.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ANF.Service.Backgrounds
{
    public sealed class SampleIdDetectionService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<SampleIdDetectionService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(2);

        public SampleIdDetectionService(IServiceScopeFactory serviceScopeFactory,
            ILogger<SampleIdDetectionService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("=================== Spam IP Detection Service started at: {time} ===================", DateTime.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckForSpamIps();
                    await PublishData();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error occurred while checking for spam IPs");
                    //throw;
                }
                await Task.Delay(_checkInterval, stoppingToken);
                _logger.LogInformation("=================== Spam IP Detection Service ompleted one iteration at: {time} ===================", DateTime.Now);
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
                                tv.ValidationStatus == ValidationStatus.Success &&
                                tv.ConversionStatus == ConversionStatus.Pending &&
                                (o.PricingModel == "CPC" || o.PricingModel == "CPA" || o.PricingModel == "CPS") &&
                                tv.ValidatedTime >= fromTime && tv.ValidatedTime <= toTime
                            select new TrackingConversionEvent
                            {
                                Id = tv.Id,
                                ClickId = tv.ClickId!,
                                PublisherCode = te.PublisherCode,
                                OfferId = te.OfferId,
                                PricingModel = o.PricingModel!,
                                Amount = tv.Amount
                            };

                var data = await query.ToListAsync();

                foreach (var item in data)
                {
                    // Use the pricing model to set the queue name
                    string queueName = item.PricingModel.ToLower(); // CPC, CPA, CPS
                    await rabbitMQPublisher.PublishAsync(queueName, item);

                    _logger.LogInformation($"=================== Published event for click_id: {item.ClickId} to queue: {queueName} ===================");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Check for spam IPs in the last 10 minutes
        /// If the IP address is duplicated more than 5 times, it will be marked as fraud
        /// Otherwise it will be marked as valid, then based on the offer's pricing model
        /// If the pricing model is CPC, the validation status will be set to success for doing conversion
        /// If the pricing model is CPA or CPS, the validation status will be set to unknown 
        /// for checking with postback from advertiser
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NoDataRetrievalException">No data retrieved from the database</exception>
        private async Task CheckForSpamIps()
        {
            _logger.LogInformation("=================== Starting spam IP check at: {time} ===================", DateTime.Now);

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
                .Include(t => t.Offer)  // Get the pricing model
                .Where(e => e.ClickTime > tenMinutesAgo && e.ClickTime <= DateTime.Now && e.Status == TrackingEventStatus.Pending)
                .ToListAsync();

            if (!trackingData.Any())
                _logger.LogInformation("=================== No tracking data found for the last 10 minutes ===================");

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
                    trackingItem.Status = TrackingEventStatus.Fraud;
                    fraudEvents.Add(trackingItem);
                    var fraudDetection = new FraudDetection
                    {
                        ClickId = trackingItem.Id,
                        Reason = $"=================== Detect duplicated IP address: {trackingItem.IpAddress} ===================",
                        DetectedTime = DateTime.Now,
                    };
                    fraudDetectionRepository.Add(fraudDetection);
                }
                else if(trackingItem.Proxy.IsNullOrEmpty() || trackingItem.Proxy == "True")
                {
                    trackingItem.Status = TrackingEventStatus.Fraud;
                    fraudEvents.Add(trackingItem);
                    var fraudDetection = new FraudDetection
                    {
                        ClickId = trackingItem.Id,
                        Reason = $"=================== Detect duplicated IP address: {trackingItem.IpAddress} ===================",
                        DetectedTime = DateTime.Now,
                    };
                    fraudDetectionRepository.Add(fraudDetection);
                }
                else
                {
                    var isValidOffer = trackingItem.Offer?.EndDate > DateTime.Now;
                    if (isValidOffer)
                    {
                        if (trackingItem.Offer?.PricingModel == "CPC")
                        {
                            trackingItem.Status = TrackingEventStatus.Valid;
                            validEvents.Add(trackingItem);
                            trackingValidationRepository.Add(new TrackingValidation
                            {
                                ClickId = trackingItem.Id,
                                ValidatedTime = DateTime.Now,
                                ValidationStatus = ValidationStatus.Success,
                                ConversionStatus = ConversionStatus.Pending,
                            });

                        }
                        else if (trackingItem.Offer?.PricingModel == "CPA" || trackingItem.Offer?.PricingModel == "CPS")
                        {
                            trackingItem.Status = TrackingEventStatus.Valid;
                            validEvents.Add(trackingItem);
                            trackingValidationRepository.Add(new TrackingValidation
                            {
                                ClickId = trackingItem.Id,
                                // Validated time is not set yet after checking with postback data
                                ValidationStatus = ValidationStatus.Unknown,
                                ConversionStatus = ConversionStatus.Pending
                            });
                        }
                    }
                    else
                    {
                        trackingItem.Status = TrackingEventStatus.Invalid;
                    }
                }
            }
            trackingEventRepository.UpdateRange(trackingData);

            await unitOfWork.SaveAsync();


            if (fraudEvents.Any())
                _logger.LogInformation("=================== Updated {count} tracking events as fraud ===================", fraudEvents.Count);
            if (validEvents.Any())
                _logger.LogInformation("=================== Updated {count} tracking events as valid ===================", validEvents.Count);
        }
    }
}
