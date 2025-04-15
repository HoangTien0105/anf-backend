using ANF.Core;
using ANF.Core.Enums;
using ANF.Core.Models.Entities;
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

        public CampaignBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<CampaignBackgroundService> logger)
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
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();
                var offerRepository = _unitOfWork.GetRepository<Offer>();

                var verifiedCampaign = await campaignRepository.GetAll()
                    .Where(e => e.Status == CampaignStatus.Verified && e.StartDate <= DateTime.Now).ToListAsync();

                if (!verifiedCampaign.Any())
                {
                    _logger.LogInformation("=================== No verified campaign found today ===================");
                }

                foreach (var v in verifiedCampaign)
                {

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
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();
                var offerRepository = _unitOfWork.GetRepository<Offer>();

                var startedCampaign = await campaignRepository.GetAll()
                    .Where(e => e.Status == CampaignStatus.Started && e.EndDate <= DateTime.Now).ToListAsync();

                if (!startedCampaign.Any())
                {
                    _logger.LogInformation("=================== No ended campaign found today ===================");
                }

                foreach (var v in startedCampaign)
                {
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
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var offerRepository = _unitOfWork.GetRepository<Offer>();

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
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var offerRepository = _unitOfWork.GetRepository<Offer>();

                var endOffer = await offerRepository.GetAll()
                    .Where(e => e.Status == OfferStatus.Started && e.EndDate <= DateTime.Now)
                    .ToListAsync();

                if (!endOffer.Any())
                {
                    _logger.LogInformation("=================== No offers to end found ===================");
                }

                foreach (var offer in endOffer)
                {
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
