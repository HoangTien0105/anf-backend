using ANF.Core;
using ANF.Core.Commons;
using ANF.Core.Enums;
using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ANF.Service
{
    public sealed class PostbackValidationService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<PostbackValidationService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromDays(1);

        public PostbackValidationService(IServiceScopeFactory serviceScopeFactory,
            ILogger<PostbackValidationService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("=================== Postback validation Service started at: {time} ===================", DateTime.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UpdateSuccessPostback();
                    await UpdateRemainPostback();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error occurred while validating for postbacks", e.StackTrace);
                    //throw;
                }
                await Task.Delay(_checkInterval, stoppingToken);
                _logger.LogInformation("=================== Completed one iteration at: {time} ===================", DateTime.Now);
            }
        }

        private async Task UpdateSuccessPostback()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            

                // Batch time window (getting data in a time range)
                var fromDate = DateTime.Now.AddDays(-8);
                var toDate = DateTime.Now;

                var postbackRepository = unitOfWork.GetRepository<PostbackData>();
                var purchaseLogRepository = unitOfWork.GetRepository<PurchaseLog>();
                var trackingValidationRepository = unitOfWork.GetRepository<TrackingValidation>();

                var postbacks = await postbackRepository.GetAll()
                    .AsNoTracking()
                    .Include(e => e.Offer)
                    .Where(e => e.Date > fromDate && e.Date <= toDate && e.Status == Core.Enums.PostbackStatus.Success)
                    .ToListAsync();

                int date = 0;
                int validValidation = 0;
                int unknownValidation = 0;
                int failValidation = 0;

                foreach (var item in postbacks)
                {
                    var trackingValidation = await trackingValidationRepository.GetAll()
                                            .FirstOrDefaultAsync(e => e.ClickId == item.ClickId);

                    if (trackingValidation is null)
                    {
                        _logger.LogWarning($"=================== Tracking validation does not exist for ClickId: {item.ClickId} ===================");
                        continue;
                    }

                    if (item.Offer is not null && item.Offer.OrderReturnTime is not null)
                    {
                        var parts = item.Offer.OrderReturnTime.Trim().Split(" ");
                        int.TryParse(parts[0], out date);
                    }

                    if (item.Date > DateTime.Now.AddDays(-date)) continue;

                    var purchaseLog = await purchaseLogRepository.GetAll()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.TransactionId == item.TransactionId);

                    if (purchaseLog is null)
                    {
                        trackingValidation.ValidationStatus = ValidationStatus.Unknown;
                        trackingValidation.ConversionStatus = ConversionStatus.Failed;
                        unknownValidation++;
                        _logger.LogInformation($"=================== Postback data doesn't exists: {item.Id} ===================");
                    }
                    else
                    {
                        if(purchaseLog.TransactionId == item.TransactionId && purchaseLog.Amount == (decimal?)item.Amount)
                        {
                            trackingValidation.ValidationStatus = ValidationStatus.Success;
                            trackingValidation.ValidatedTime = DateTime.Now;
                            validValidation++;
                        }
                        else
                        {
                            trackingValidation.ValidationStatus = ValidationStatus.Failed;
                            trackingValidation.ConversionStatus = ConversionStatus.Failed;
                            failValidation++;
                            _logger.LogInformation($"=================== Postback data does not match the postback logsd: {item.Id} ===================");
                        }
                    }

                    trackingValidationRepository.Update(trackingValidation);
                }

                await unitOfWork.SaveAsync();
                _logger.LogInformation("=================== Updated tracking validations: {ValidCount} valid, {FailCount} failed, {UnknownCount} unknown ===================",
                                        validValidation, failValidation, unknownValidation);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                throw;
            }
        }

        private async Task UpdateRemainPostback()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();


                // Batch time window (getting data in a time range)
                var fromDate = DateTime.Now.AddDays(-8);
                var toDate = DateTime.Now;

                var postbackRepository = unitOfWork.GetRepository<PostbackData>();
                var purchaseLogRepository = unitOfWork.GetRepository<PurchaseLog>();
                var trackingValidationRepository = unitOfWork.GetRepository<TrackingValidation>();

                var postbacks = await postbackRepository.GetAll()
                    .AsNoTracking()
                    .Include(e => e.Offer)
                    .Where(e => e.Date > fromDate && e.Date <= toDate && e.Status != Core.Enums.PostbackStatus.Success)
                    .ToListAsync();

                int date = 0;

                int unknownValidation = 0;
                int failValidation = 0;

                foreach (var item in postbacks)
                {
                    var trackingValidation = await trackingValidationRepository.GetAll()
                                            .FirstOrDefaultAsync(e => e.ClickId == item.ClickId);

                    if (trackingValidation is null)
                    {
                        _logger.LogWarning($"=================== Tracking validation does not exist for ClickId: {item.ClickId} ===================");
                        continue;
                    }

                    if (item.Offer is not null && item.Offer.OrderReturnTime is not null)
                    {
                        var parts = item.Offer.OrderReturnTime.Trim().Split(" ");
                        int.TryParse(parts[0], out date);
                    }

                    if (item.Date > DateTime.Now.AddDays(-date)) continue;

                    if(item.Status == PostbackStatus.Failed || item.Status == PostbackStatus.Refunded || item.Status == PostbackStatus.Canceled)
                    {
                        trackingValidation.ValidationStatus = ValidationStatus.Failed;
                        trackingValidation.ConversionStatus = ConversionStatus.Failed;
                        failValidation++;
                    } 
                    else
                    {
                        trackingValidation.ValidationStatus = ValidationStatus.Unknown;
                        trackingValidation.ConversionStatus = ConversionStatus.Failed;
                        unknownValidation++;
                    }

                    trackingValidationRepository.Update(trackingValidation);
                }

                await unitOfWork.SaveAsync();

                _logger.LogInformation("=================== Updated tracking validations: {FailCount} failed, {UnknownCount} unknown ===================",
                                        failValidation, unknownValidation);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                throw;
            }
        }
    }
}
