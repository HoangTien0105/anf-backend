using ANF.Core;
using ANF.Core.Commons;
using ANF.Core.Enums;
using ANF.Core.Models.Entities;
using ANF.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ANF.Service
{
    public sealed class CampaignBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<CampaignBackgroundService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5);

        public CampaignBackgroundService(IServiceScopeFactory serviceScopeFactory, 
                                         ILogger<CampaignBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("=================== CampaignBackgroundService is starting. ===================");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ValidateStartedCampaign();
                    await ValidateEndedCampaign();
                    await ValidateStartedOffer();
                    await ValidateEndedOffer();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "=================== An error occurred while validating campaigns. ===================");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("=================== CampaignBackgroundService is stopping. ===================");
        }

        private async Task ValidateStartedCampaign()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();
                var userRepository = _unitOfWork.GetRepository<User>();
                var offerRepository = _unitOfWork.GetRepository<Offer>();

                var verifiedCampaign = await campaignRepository.GetAll()
                    .Where(e => e.Status == CampaignStatus.Verified && e.StartDate <= DateTime.Now).ToListAsync();

                if (!verifiedCampaign.Any())
                {
                    _logger.LogInformation("=================== No verified campaign found today ===================");
                }

                foreach (var v in verifiedCampaign)
                {
                    var user = await userRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.UserCode == v.AdvertiserCode);
                    if (user is null) throw new KeyNotFoundException("Advertiser does not exist!");

                    var message = new EmailMessage
                    {
                        To = user.Email,
                        Subject = "Campaign notifications"
                    };

                    var emailResult = await _emailService.SendCampaignNotificationEmail(message, v.Name, null, "Started");
                    if (!emailResult)
                    {
                        _logger.LogInformation("=================== Failed to send email for campaign notifications! ===================");
                        continue;
                    }

                    v.Status = CampaignStatus.Started;
                    var offers = await offerRepository.GetAll()
                        .Where(e => e.Status == OfferStatus.Approved && e.CampaignId == v.Id).ToListAsync();

                    foreach (var offer in offers)
                    {
                        if (offer.StartDate <= DateTime.Now)
                        {
                            offer.Status = OfferStatus.Started;
                        }
                    }
                    offerRepository.UpdateRange(offers);
                }
                campaignRepository.UpdateRange(verifiedCampaign);

                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task ValidateEndedCampaign()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();
                var userRepository = _unitOfWork.GetRepository<User>();
                var offerRepository = _unitOfWork.GetRepository<Offer>();

                var startedCampaign = await campaignRepository.GetAll()
                    .Where(e => e.Status == CampaignStatus.Started && e.EndDate <= DateTime.Now).ToListAsync();

                if (!startedCampaign.Any())
                {
                    _logger.LogInformation("=================== No ended campaign found today ===================");
                }

                foreach (var v in startedCampaign)
                {
                    var user = await userRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.UserCode == v.AdvertiserCode);
                    if (user is null) throw new KeyNotFoundException("Advertiser does not exist!");

                    var message = new EmailMessage
                    {
                        To = user.Email,
                        Subject = "Campaign notifications"
                    };

                    var emailResult = await _emailService.SendCampaignNotificationEmail(message, v.Name, null, "Ended");
                    if (!emailResult)
                    {
                        _logger.LogInformation("=================== Failed to send email for campaign notifications! ===================");
                        continue;
                    }

                    v.Status = CampaignStatus.Ended;
                    var offers = await offerRepository.GetAll()
                        .Where(e => (e.Status == OfferStatus.Started || e.Status == OfferStatus.Approved)
                        && e.CampaignId == v.Id)
                        .ToListAsync();

                    foreach (var offer in offers)
                    {

                        offer.Status = OfferStatus.Ended;

                    }
                    offerRepository.UpdateRange(offers);
                }
                campaignRepository.UpdateRange(startedCampaign);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task ValidateStartedOffer()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var offerRepository = _unitOfWork.GetRepository<Offer>(); 
                var userRepository = _unitOfWork.GetRepository<User>();

                var approveOffer = await offerRepository.GetAll()
                    .Where(e => e.Status == OfferStatus.Approved && e.StartDate <= DateTime.Now)
                    .Include(e => e.Campaign)
                    .Where(e => e.Campaign.Status == CampaignStatus.Started)
                    .ToListAsync();

                if (!approveOffer.Any())
                {
                    _logger.LogInformation("=================== No offers to start found ===================");
                }

                foreach (var offer in approveOffer)
                {
                    var user = await userRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.UserCode == offer.Campaign.AdvertiserCode);
                    if (user is null) throw new KeyNotFoundException("Advertiser does not exist!");

                    var message = new EmailMessage
                    {
                        To = user.Email,
                        Subject = "Campaign notifications"
                    };

                    var emailResult = await _emailService.SendCampaignNotificationEmail(message, offer.Campaign.Name, offer.Id, "Started");
                    if (!emailResult)
                    {
                        _logger.LogInformation("=================== Failed to send email for campaign notifications! ===================");
                        continue;
                    }
                    offer.Status = OfferStatus.Started;
                }

                offerRepository.UpdateRange(approveOffer);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task ValidateEndedOffer()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var userRepository = _unitOfWork.GetRepository<User>();
                var offerRepository = _unitOfWork.GetRepository<Offer>();

                var endOffer = await offerRepository.GetAll()
                    .Where(e => e.Status == OfferStatus.Started && e.EndDate <= DateTime.Now)
                    .Include(e => e.Campaign)
                    .ToListAsync();

                if (!endOffer.Any())
                {
                    _logger.LogInformation("=================== No offers to end found ===================");
                }

                foreach (var offer in endOffer)
                {
                    var user = await userRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.UserCode == offer.Campaign.AdvertiserCode);
                    if (user is null) throw new KeyNotFoundException("Advertiser does not exist!");

                    var message = new EmailMessage
                    {
                        To = user.Email,
                        Subject = "Campaign notifications"
                    };

                    var emailResult = await _emailService.SendCampaignNotificationEmail(message, offer.Campaign.Name, offer.Id, "Ended");
                    if (!emailResult)
                    {
                        _logger.LogInformation("=================== Failed to send email for campaign notifications! ===================");
                        continue;
                    }
                    offer.Status = OfferStatus.Ended;
                }

                offerRepository.UpdateRange(endOffer);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
