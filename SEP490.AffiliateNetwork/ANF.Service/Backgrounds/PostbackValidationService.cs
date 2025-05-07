using ANF.Core;
using ANF.Core.Commons;
using ANF.Core.Enums;
using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ANF.Service.Backgrounds
{
    public sealed class PostbackValidationService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<PostbackValidationService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

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
                _logger.LogInformation("=================== Postback validation Service completed one iteration at: {time} ===================", DateTime.Now);
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
                            select new 
                            {
                                PostbackData = pb,
                                TrackingValidation = tv
                            };
                                    
                var postbacks = await query.ToListAsync();
                int validValidation = 0;

                // Check từng postback
                foreach (var item in postbacks)
                {
                    var trackingValidation = item.TrackingValidation;
                    // LỖi nếu không có tracking validtion cho postback
                    if (trackingValidation is null)
                    {
                        _logger.LogWarning($"=================== Tracking validation does not exist for ClickId: {item.PostbackData.ClickId} ===================");
                        continue;
                    }
                    else
                    {
                        trackingValidation.ValidationStatus = ValidationStatus.Success;
                        trackingValidation.Amount = (decimal?) item.PostbackData.Amount;
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
                                .Include(pb => pb.Offer)
                            on tv.ClickId equals pb.ClickId
                            where tv.ConversionStatus == ConversionStatus.Pending
                            && pb.Status != PostbackStatus.Success
                            select new
                            {
                                PostbackData = pb,
                                TrackingValidation = tv
                            };

                var postbacks = await query.ToListAsync();

                int date = 0;

                int validValidation = 0;
                int failValidation = 0;

                foreach (var item in postbacks)
                {
                    var trackingValidation = item.TrackingValidation;

                    if (trackingValidation is null)
                    {
                        _logger.LogWarning($"=================== Tracking validation does not exist for ClickId: {item.PostbackData.ClickId} ===================");
                        continue;
                    }

                    if (item.PostbackData.Offer is not null && item.PostbackData.Offer.OrderReturnTime is not null)
                    {
                        var parts = item.PostbackData.Offer.OrderReturnTime.Trim().Split(" ");
                        int.TryParse(parts[0], out date);
                    }

                    //Use for demo, use AddDays for production
                    if (item.PostbackData.Date > DateTime.Now.AddMinutes(-date)) continue;

                    if(item.PostbackData.Status == PostbackStatus.Failed || item.PostbackData.Status == PostbackStatus.Refunded || item.PostbackData.Status == PostbackStatus.Canceled)
                    {
                        trackingValidation.ValidationStatus = ValidationStatus.Failed;
                        trackingValidation.ConversionStatus = ConversionStatus.Failed;
                        trackingValidation.ValidatedTime = DateTime.Now;
                        failValidation++;
                    } 
                    else
                    {
                        item.PostbackData.Status = PostbackStatus.Success;
                        trackingValidation.ValidationStatus = ValidationStatus.Success;
                        trackingValidation.ConversionStatus = ConversionStatus.Pending;
                        trackingValidation.ValidatedTime = DateTime.Now;
                        validValidation++;
                    }

                    postbackRepository.Update(item.PostbackData);
                    trackingValidationRepository.Update(trackingValidation);
                }

                await unitOfWork.SaveAsync();

                _logger.LogInformation("=================== Updated tracking validations: {FailCount} failed, {ValidCount} unknown ===================",
                                        failValidation, validValidation);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.StackTrace);
                throw;
            }
        }
    }
}
