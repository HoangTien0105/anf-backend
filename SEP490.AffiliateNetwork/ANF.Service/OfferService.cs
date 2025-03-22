using ANF.Core;
using ANF.Core.Exceptions;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ANF.Core.Enums;
using ANF.Core.Commons;

namespace ANF.Service
{
    public class OfferService(IUnitOfWork unitOfWork, IMapper mapper,
        ICloudinaryService cloudinaryService,
        IUserClaimsService userClaimsService) : IOfferService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly IUserClaimsService _userClaimsService = userClaimsService;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> ApplyOffer(string pubId, long offerId)
        {
            try
            {
                var currentPublisherCode = _userClaimsService.GetClaim(ClaimConstants.NameId);
                if (currentPublisherCode != pubId)
                    throw new UnauthorizedAccessException("Publisher's code does not match!");
                if (pubId is null) throw new NullReferenceException("Invalid request data. Please check again!");

                var offerRepository = _unitOfWork.GetRepository<Offer>();
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();
                var pubOfferRepository = _unitOfWork.GetRepository<PublisherOffer>();
                var userRepository = _unitOfWork.GetRepository<User>();

                var offerExist = await offerRepository.GetAll()
                                        .AsNoTracking().FirstOrDefaultAsync(e => e.Id == offerId);
                if (offerExist is null) throw new KeyNotFoundException("Offer does not exists");

                var publisherExist = await userRepository.GetAll()
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(e => e.UserCode.ToString() == pubId && e.Role == UserRoles.Publisher);
                if (publisherExist is null) throw new KeyNotFoundException("Publisher does not exists");

                var campaignExist = await campaignRepository.GetAll()
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(e => e.Id == offerExist.CampaignId);
                if (campaignExist is null) throw new KeyNotFoundException("Campaign does not exists");

                if (campaignExist.Status != CampaignStatus.Verified && campaignExist.Status != CampaignStatus.Started)
                    throw new InvalidOperationException("Campaign must be Verified or Started for offer to be applied");

                var pubOfferExist = await pubOfferRepository.GetAll()
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(e => e.OfferId == offerId && e.PublisherCode.ToString() == pubId);
                if (pubOfferExist is not null) throw new DuplicatedException("You have applied this offer alreay!");

                PublisherOffer publisherOffer = new PublisherOffer
                {
                    OfferId = offerId,
                    PublisherCode = publisherExist.UserCode,
                    JoiningDate = DateTime.UtcNow,
                    Status = PublisherOfferStatus.Pending
                };

                pubOfferRepository.Add(publisherOffer);
                var affectedRows = await _unitOfWork.SaveAsync();
                return affectedRows > 0;

            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> CreateOffer(OfferCreateRequest request)
        {
            try
            {
                if (request is null) throw new NullReferenceException("Invalid request data. Please check again!");
                var offerRepository = _unitOfWork.GetRepository<Offer>();
                var imageRepository = _unitOfWork.GetRepository<CampaignImage>();
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();

                var duplicatedOffer = await offerRepository.GetAll()
                                        .AsNoTracking()
                                        .AnyAsync(e => e.CampaignId == request.CampaignId &&
                                        e.StartDate == request.StartDate &&
                                        e.EndDate == request.EndDate &&
                                        e.Bid == request.Bid &&
                                        e.Budget == request.Budget &&
                                        e.PricingModel == request.PricingModel);

                if (duplicatedOffer) throw new DuplicatedException("Offer already exists");

                var campaignExist = await campaignRepository.GetAll()
                                        .AsNoTracking()
                                        .Where(e => e.Id == request.CampaignId)
                                        .FirstOrDefaultAsync();
                if (campaignExist is null) throw new KeyNotFoundException("Campaign does not exists");

                if (campaignExist.Status != CampaignStatus.Pending)
                    throw new ArgumentException("Campaign status must be Pending to create offer");

                var validModel = PricingModelConstant.pricingModels.Any(e => e.Name.Trim() == request.PricingModel.Trim());
                if (!validModel) throw new KeyNotFoundException("Pricing model does not exists");

                if (request.StartDate < campaignExist.StartDate || request.StartDate > campaignExist.EndDate)
                    throw new ArgumentException("Offer start date must be between start and end date of campaign");

                if (request.EndDate < campaignExist.StartDate && request.EndDate > campaignExist.EndDate)
                    throw new ArgumentException("Offer end date must be between start and end date of campaign");

                if (request.EndDate <= request.StartDate)
                    throw new ArgumentException("End date must be after start date");

                if (request.Bid >= request.Budget)
                {
                    throw new ArgumentException("Bid can't higher than budget");
                }

                var offer = _mapper.Map<Offer>(request);

                if (request.OfferImages is not null)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(request.OfferImages);
                    if (imageUrl is not null)
                    {
                        offer.ImageUrl = imageUrl;
                    }
                    else
                    {
                        throw new ArgumentException("Something went wrong with image");
                    }
                }
                offerRepository.Add(offer);
                campaignExist.Balance += offer.Budget;
                campaignRepository.Update(campaignExist);

                var affectedRows = await _unitOfWork.SaveAsync();
                return affectedRows > 0;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteOffer(long id)
        {
            try
            {
                var offerRepository = _unitOfWork.GetRepository<Offer>();
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();
                var offer = await offerRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (offer is null)
                    throw new KeyNotFoundException("Offer does not exist!");

                var campaign = await campaignRepository.GetAll()
                                        .AsNoTracking()
                                        .Where(e => e.Id == offer.CampaignId)
                                        .Include(e => e.Offers)
                                        .FirstOrDefaultAsync();

                if (campaign is null)
                    throw new KeyNotFoundException("Campaign does not exists");

                if (campaign.Status != CampaignStatus.Pending)
                    throw new InvalidOperationException("Campaign status must be Pending to delete offer");

                if (campaign.Offers.Count < 2)
                    throw new InvalidOperationException("Campaign must have at least 1 offer");

                offerRepository.Delete(offer);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<OfferResponse> GetOffer(long offerId)
        {
            var offerRepository = _unitOfWork.GetRepository<Offer>();
            var offer = await offerRepository.GetAll()
                            .AsNoTracking()
                            .FirstOrDefaultAsync(e => e.Id == offerId);
            if (offer is null)
                throw new KeyNotFoundException("Offer does not exist!");
            var response = _mapper.Map<OfferResponse>(offer);
            return response;
        }

        public async Task<PaginationResponse<OfferResponse>> GetOffers(PaginationRequest request)
        {
            var offerRepository = _unitOfWork.GetRepository<Offer>();
            var offers = await offerRepository.GetAll()
                            .AsNoTracking()
                            .Skip((request.pageNumber - 1) * request.pageSize)
                            .Take(request.pageSize)
                            .ToListAsync();
            if (!offers.Any())
                throw new KeyNotFoundException("No data for offers!");
            var totalCounts = offers.Count();

            var data = _mapper.Map<List<OfferResponse>>(offers);
            return new PaginationResponse<OfferResponse>(data, totalCounts, request.pageNumber, request.pageSize);
        }

        public async Task<List<PublisherOfferResponse>> GetPublisherOfOffer(long offerId)
        {
            var publisherOffer = _unitOfWork.GetRepository<PublisherOffer>();
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();

            var advertiserCode = _userClaimsService.GetClaim(ClaimConstants.NameId);
            if (string.IsNullOrEmpty(advertiserCode))
                throw new UnauthorizedAccessException("Advertiser's code is empty!");

            // Check whether the offer is belong to the current advertiser
            var offers = await campaignRepository.GetAll()
                .AsNoTracking()
                .Include(c => c.Offers)
                .Where(c => c.AdvertiserCode == advertiserCode)
                .SelectMany(c => c.Offers)
                .ToListAsync();
            var isExisted = offers.Any(o => o.Id == offerId);
            if (!isExisted)
                throw new ForbiddenException("The current advertiser is not the owner of this offer!");

            var publishers = await publisherOffer.GetAll()
                .AsNoTracking()
                .Include(po => po.Publisher)
                .ThenInclude(p => p.AffiliateSources)
                .Where(po => po.OfferId == offerId)
                .Select(x => new
                {
                    x.Id,
                    PublisherId =  x.Publisher.Id,
                    x.PublisherCode,
                    x.OfferId,
                    FirstName = x.Publisher.FirstName,
                    LastName = x.Publisher.LastName,
                    PhoneNumber = x.Publisher.PhoneNumber,
                    Email = x.Publisher.Email,
                    TrafficSources = x.Publisher.AffiliateSources.Select(y => new
                    {
                        y.Provider,
                        y.SourceUrl,
                        y.Type
                    }).ToList()
                })
                .ToListAsync();
            if (!publishers.Any())
                throw new NoDataRetrievalException("No data of publishers!");

            var responses = new List<PublisherOfferResponse>();
            foreach (var item in publishers)
            {
                var response = new PublisherOfferResponse()
                {
                    Id = item.Id,
                    PublisherId = item.PublisherId,
                    OfferId = item.OfferId,
                    PublisherCode = item.PublisherCode,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    PhoneNumber = item.PhoneNumber,
                    Email = item.Email,
                    TrafficSources = item.TrafficSources.Select(x => new PublisherOfferTrafficSource
                    {
                        Provider = x.Provider,
                        SourceUrl = x.SourceUrl,
                        Type = x.Type
                    }).ToList(),
                };
                responses.Add(response);
            }
            return responses;
        }

        public async Task<bool> UpdateApplyOfferStatus(long pubOfferId, string status, string? rejectReason)
        {
            try
            {
                var pubOfferRepository = _unitOfWork.GetRepository<PublisherOffer>();
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();
                var offerRepository = _unitOfWork.GetRepository<Offer>();

                var pubOfferExist = await pubOfferRepository.GetAll()
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(e => e.Id == pubOfferId);
                if (pubOfferExist is null) throw new KeyNotFoundException("Publisher request for offer doest not exist.");

                var offerExist = await offerRepository.GetAll()
                                        .AsNoTracking().FirstOrDefaultAsync(e => e.Id == pubOfferExist.OfferId);
                if (offerExist is null) throw new KeyNotFoundException("Offer does not exists");

                var campaignExist = await campaignRepository.GetAll()
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(e => e.Id == offerExist.CampaignId &&
                                            (e.Status == CampaignStatus.Pending
                                            || e.Status == CampaignStatus.Verified
                                            || e.Status == CampaignStatus.Started));
                if (campaignExist is null) throw new KeyNotFoundException("Campaign must be Pending or Verified for offer to be updated");

                if (!Enum.TryParse<PublisherOfferStatus>(status, true, out var pubOfferStatus))
                    throw new ArgumentException("Invalid offer's status. Please check again!");

                pubOfferExist.Status = pubOfferStatus;
                pubOfferExist.JoiningDate = DateTime.UtcNow;    //TODO: Fix (joining_date phải là ngày publisher apply, k update field này ở method này)
                                                                // Db có thể bổ sung thêm 1 field approved_date, chỗ này đưa value vào
                if (pubOfferStatus == PublisherOfferStatus.Rejected)
                {
                    pubOfferExist.RejectReason = rejectReason;
                }

                pubOfferRepository.Update(pubOfferExist);
                return await _unitOfWork.SaveAsync() > 0;

            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateOffer(long id, OfferUpdateRequest request)
        {
            try
            {
                var offerRepository = _unitOfWork.GetRepository<Offer>();
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();

                if (request is null) throw new NullReferenceException("Invalid request data. Please check again!");
                var offer = await offerRepository.GetAll()
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(e => e.Id == id);
                if (offer is null) throw new NullReferenceException("Offer does not exists");

                var campaignExist = await campaignRepository.GetAll()
                                        .AsNoTracking()
                                        .Where(e => e.Id == offer.CampaignId)
                                        .FirstOrDefaultAsync();
                if (campaignExist is null) throw new KeyNotFoundException("Campaign does not exists");

                if (campaignExist.Status != CampaignStatus.Pending)
                    throw new ArgumentException("Campaign status must be Pending to update offer");

                var validModel = PricingModelConstant.pricingModels.Any(e => e.Name.Trim() == request.PricingModel.Trim());
                if (!validModel) throw new KeyNotFoundException("Pricing model does not exists");

                if (request.StartDate < campaignExist.StartDate && request.StartDate > campaignExist.EndDate)
                    throw new ArgumentException("Offer start date must be between start and end date of campaign");

                if (request.EndDate < campaignExist.StartDate && request.EndDate > campaignExist.EndDate)
                    throw new ArgumentException("Offer end date must be between start and end date of campaign");

                if (request.EndDate <= request.StartDate)
                    throw new ArgumentException("End date must be after start date");

                if (request.Bid >= request.Budget)
                {
                    throw new ArgumentException("Bid can't higher than budget");
                }

                _ = _mapper.Map(request, offer);

                if (request.OfferImages is not null)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(request.OfferImages);
                    if (imageUrl is not null)
                    {
                        offer.ImageUrl = imageUrl;
                    }
                    else
                    {
                        throw new ArgumentException("Something went wrong with image");
                    }
                }

                offerRepository.Update(offer);

                var existingOffersSum = await offerRepository.GetAll()
                                            .AsNoTracking()
                                            .Where(e => e.CampaignId == campaignExist.Id && e.Id != offer.Id)
                                            .SumAsync(e => e.Budget);

                campaignExist.Balance = existingOffersSum + offer.Budget;

                campaignRepository.Update(campaignExist);

                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
