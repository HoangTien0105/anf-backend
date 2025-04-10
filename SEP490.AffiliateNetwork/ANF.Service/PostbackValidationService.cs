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
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(2);

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
        
        //Nếu postback success là + tiền
        private async Task UpdateSuccessPostback()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            

                // Batch time window (getting data in a time range)


                var postbackRepository = unitOfWork.GetRepository<PostbackData>();
                var purchaseLogRepository = unitOfWork.GetRepository<PurchaseLog>();
                var trackingValidationRepository = unitOfWork.GetRepository<TrackingValidation>();

                // Lấy postback status success có tracking validtion conversion ở status pending
                var query = from tv in trackingValidationRepository
                                .GetAll()
                                .AsNoTracking()
                            join pb in postbackRepository
                                .GetAll()
                                .AsNoTracking()
                            on tv.ClickId equals pb.ClickId
                            where tv.ConversionStatus == ConversionStatus.Pending
                            && pb.Status == PostbackStatus.Success
                            select new PostbackData
                            {
                                Id = pb.Id,
                                ClickId = pb.ClickId,
                                OfferId = pb.OfferId,
                                TransactionId = pb.TransactionId,
                                Date = pb.Date,
                                PublisherCode = pb.PublisherCode,
                                Amount = pb.Amount,
                                Status = pb.Status,
                            };
                                    
                var postbacks = await query.ToListAsync();
                int validValidation = 0;

                // Check từng postback
                foreach (var item in postbacks)
                {
                    var trackingValidation = await trackingValidationRepository.GetAll()
                                            .FirstOrDefaultAsync(e => e.ClickId == item.ClickId);
                    // LỖi nếu không có tracking validtion cho postback
                    if (trackingValidation is null)
                    {
                        _logger.LogWarning($"=================== Tracking validation does not exist for ClickId: {item.ClickId} ===================");
                        continue;
                    }
                    else
                    {
                        trackingValidation.ValidationStatus = ValidationStatus.Success;
                        trackingValidation.Amount = (decimal?) item.Amount;
                        trackingValidation.ValidatedTime = DateTime.Now;
                        trackingValidationRepository.Update(trackingValidation);
                    }
                }
                await unitOfWork.SaveAsync();
                _logger.LogInformation("=================== Updated tracking validations: {ValidCount} valid ===================", validValidation);
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

                var postbackRepository = unitOfWork.GetRepository<PostbackData>();
                var purchaseLogRepository = unitOfWork.GetRepository<PurchaseLog>();
                var trackingValidationRepository = unitOfWork.GetRepository<TrackingValidation>();

                var query = from tv in trackingValidationRepository
                                                .GetAll()
                                                .AsNoTracking()
                            join pb in postbackRepository
                                .GetAll()
                                .AsNoTracking()
                            on tv.ClickId equals pb.ClickId
                            where tv.ConversionStatus == ConversionStatus.Pending
                            && pb.Status != PostbackStatus.Success
                            select new PostbackData
                            {
                                Id = pb.Id,
                                ClickId = pb.ClickId,
                                OfferId = pb.OfferId,
                                TransactionId = pb.TransactionId,
                                Date = pb.Date,
                                PublisherCode = pb.PublisherCode,
                                Amount = pb.Amount,
                                Status = pb.Status,
                            };

                var postbacks = await query.ToListAsync();

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
                        trackingValidation.ValidatedTime = DateTime.Now;
                        failValidation++;
                    } 
                    else
                    {
                        trackingValidation.ValidationStatus = ValidationStatus.Unknown;
                        trackingValidation.ConversionStatus = ConversionStatus.Failed;
                        trackingValidation.ValidatedTime = DateTime.Now;
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
