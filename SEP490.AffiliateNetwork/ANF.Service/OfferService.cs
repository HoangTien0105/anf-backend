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
        IUserClaimsService userClaimsService,
        INotificationService notificationService,
        IEmailService emailService) : IOfferService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly INotificationService _notificationService = notificationService;
        private readonly IUserClaimsService _userClaimsService = userClaimsService;
        private readonly IEmailService _emailService = emailService;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> ApplyOffer(long offerId)
        {
            try
            {
                var pubId = _userClaimsService.GetClaim(ClaimConstants.NameId);
                var offerRepository = _unitOfWork.GetRepository<Offer>();
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();
                var pubOfferRepository = _unitOfWork.GetRepository<PublisherOffer>();
                var userRepository = _unitOfWork.GetRepository<User>();

                var publisherExist = await userRepository.GetAll()
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(e => e.UserCode.ToString() == pubId);
                if (publisherExist is null || publisherExist.Role != UserRoles.Publisher) 
                    throw new ForbiddenException("This user does not have access permission");

                var offerExist = await offerRepository.GetAll()
                                        .AsNoTracking().FirstOrDefaultAsync(e => e.Id == offerId);
                if (offerExist is null) throw new KeyNotFoundException("Offer does not exists");


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
                    JoiningDate = DateTime.Now,
                    Status = PublisherOfferStatus.Pending
                };

                pubOfferRepository.Add(publisherOffer);
                var affectedRows = await _unitOfWork.SaveAsync();
                if(affectedRows > 0) {
                    await _notificationService.NotifyRequestToJoinOffer(campaignExist.AdvertiserCode, "There is a request to join your offer", campaignExist.Id, offerId);

                    var advertiser = await userRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.UserCode == campaignExist.AdvertiserCode);

                    var message = new EmailMessage
                    {
                        To = advertiser!.Email,
                        Subject = "Campaign notifications",
                        Body = "There is a request to join your offer (Campaign: " + campaignExist.Id + ")"
                    };

                    var emailResult = await _emailService.SendNotificationEmailForAdvertiser(message, campaignExist.Id, offerId);
                    return true;
                } 
                else
                {
                    return false;
                }

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
                var walletRepository = _unitOfWork.GetRepository<Wallet>();

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

                var advWallet = await walletRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.UserCode == campaignExist.AdvertiserCode);
                if (advWallet is null) throw new KeyNotFoundException("Advertiser wallet does not exists");

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

                if(request.CommissionRate is not null)
                {
                    if(request.CommissionRate <= 0) throw new ArgumentException("Commission rate must be greater than 0");
                }

                if (request.PricingModel == "CPS" && request.CommissionRate is null) throw new ArgumentException("Pricing model CPS must have Commission rate");

                var offer = _mapper.Map<Offer>(request);

                if(request.OrderReturnTime is not null)
                {
                    offer.OrderReturnTime = request.OrderReturnTime + " days";
                }

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
                // Cộng thêm budget của offer vào budget của campaign và đem đi so sánh
                campaignExist.Budget += offer.Budget;

                if (campaignExist.Budget > advWallet.Balance)
                    throw new ArgumentException("Advertiser does not have enough for this campaign.");

                var advCampaignMoney = await campaignRepository.GetAll()
                        .AsNoTracking()
                        .Where(e => e.AdvertiserCode == campaignExist.AdvertiserCode
                        && e.Id != campaignExist.Id
                        && (e.Status == CampaignStatus.Started
                        || e.Status == CampaignStatus.Verified
                        || e.Status == CampaignStatus.Pending))
                        .SumAsync(e => e.Budget);

                if (advCampaignMoney + campaignExist.Budget > advWallet.Balance)
                    throw new ArgumentException("Advertiser does not have enough balance to cover all campaigns.");

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
                    Status = x.Status,
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
                    PubOfferStatus = (int)item.Status,
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

        public async Task<bool> ApplyPublisherOffer(long pubOfferId, string status, string? rejectReason)
        {
            try
            {
                var pubOfferRepository = _unitOfWork.GetRepository<PublisherOffer>();
                var userRepository = _unitOfWork.GetRepository<User>();
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();
                var offerRepository = _unitOfWork.GetRepository<Offer>();
                var advertiserCode = _userClaimsService.GetClaim(ClaimConstants.NameId);

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
                if (campaignExist is null) throw new KeyNotFoundException("Campaign must be Pending, Verified or Started for offer to be updated");

                if(campaignExist.AdvertiserCode != advertiserCode)
                    throw new ForbiddenException("The current advertiser is not the owner of this offer!");

                if (!Enum.TryParse<PublisherOfferStatus>(status, true, out var pubOfferStatus))
                    throw new ArgumentException("Invalid offer's status. Please check again!");

                pubOfferExist.Status = pubOfferStatus;

                if(pubOfferStatus == PublisherOfferStatus.Approved) pubOfferExist.ApprovedDate = DateTime.Now;    

                if (pubOfferStatus == PublisherOfferStatus.Rejected)
                {
                    pubOfferExist.RejectReason = rejectReason;
                } 

                pubOfferRepository.Update(pubOfferExist);
                var updatedRow = await _unitOfWork.SaveAsync() > 0;

                if (updatedRow)
                {
                    //Notify to publ
                    await _notificationService.NotifyPublisherOffer(pubOfferExist.PublisherCode, pubOfferId, pubOfferExist.Status.ToString(), rejectReason);

                    var publisher = await userRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.UserCode == pubOfferExist.PublisherCode);

                    var message = new EmailMessage
                    {
                        To = publisher!.Email,
                        Subject = "Campaign notifications",
                        Body = "Advertiser have accepted your request to join offer (Campaign: " + campaignExist.Id + ")"
                    };

                    var emailResult = await _emailService.SendNotificationEmailForPublisher(message, campaignExist.Id);

                    if (!emailResult) throw new Exception("Failed to send email!");
                    return true;
                }
                else
                {
                    return false;
                }

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
                var walletRepository = _unitOfWork.GetRepository<Wallet>();

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

                var advWallet = await walletRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.UserCode == campaignExist.AdvertiserCode);
                if (advWallet is null) throw new KeyNotFoundException("Advertiser wallet does not exists");

                var validModel = PricingModelConstant.pricingModels.Any(e => e.Name.Trim() == request.PricingModel.Trim());
                if (!validModel) throw new KeyNotFoundException("Pricing model does not exists");

                if (request.StartDate < campaignExist.StartDate && request.StartDate > campaignExist.EndDate)
                    throw new ArgumentException("Offer start date must be between start and end date of campaign");

                if (request.EndDate < campaignExist.StartDate || request.EndDate > campaignExist.EndDate)
                    throw new ArgumentException("Offer end date must be between start and end date of campaign");

                if (request.EndDate <= request.StartDate)
                    throw new ArgumentException("End date must be after start date");

                if (request.Bid >= request.Budget)
                {
                    throw new ArgumentException("Bid can't higher than budget");
                }

                if(request.CommissionRate is not null)
                {
                    if (request.CommissionRate <= 0) throw new ArgumentException("Commission rate must be greater than 0");
                }

                if (request.PricingModel == "CPS" && request.CommissionRate is null) throw new ArgumentException("Pricing model CPS must have Commission rate");

                //Trừ budget cũ của offer trước
                campaignExist.Budget -= offer.Budget;

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

                if (request.OrderReturnTime is not null)
                {
                    offer.OrderReturnTime = request.OrderReturnTime + " days";
                }

                offerRepository.Update(offer);

                var existingOffersSum = await offerRepository.GetAll()
                                            .AsNoTracking()
                                            .Where(e => e.CampaignId == campaignExist.Id && e.Id != offer.Id)
                                            .SumAsync(e => e.Budget);

                campaignExist.Balance = existingOffersSum + offer.Budget;
                campaignExist.Budget += offer.Budget;

                if (campaignExist.Budget > advWallet.Balance)
                    throw new ArgumentException("Advertiser does not have enough for this campaign.");

                var advCampaignMoney = await campaignRepository.GetAll()
                        .AsNoTracking()
                        .Where(e => e.AdvertiserCode == campaignExist.AdvertiserCode
                        && e.Id != campaignExist.Id
                        && (e.Status == CampaignStatus.Started
                        || e.Status == CampaignStatus.Verified
                        || e.Status == CampaignStatus.Pending))
                        .SumAsync(e => e.Budget);

                if (advCampaignMoney + campaignExist.Budget > advWallet.Balance)
                    throw new ArgumentException("Advertiser does not have enough balance to cover all campaigns.");
                
                campaignRepository.Update(campaignExist);

                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<List<OfferResponse>> GetOffersByPublisher()
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var pubOfferRepository = _unitOfWork.GetRepository<PublisherOffer>();
            var offerRepository = _unitOfWork.GetRepository<Offer>();
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();

            var publisherCode = _userClaimsService.GetClaim(ClaimConstants.NameId);
            if (string.IsNullOrEmpty(publisherCode))
                throw new UnauthorizedAccessException("Publisher's code is empty!");

            var pubOffers = await pubOfferRepository.GetAll()
                                .AsNoTracking()
                                .Where(e => e.PublisherCode == publisherCode 
                                && (e.Status == PublisherOfferStatus.Pending || e.Status == PublisherOfferStatus.Approved))
                                .ToListAsync();

            if (!pubOffers.Any()) throw new NoDataRetrievalException("This publisher doesn't apply offer");

            var responses = new List<OfferResponse>();
            foreach (var i in pubOffers)
            {
                var offer = await offerRepository.GetAll()
                                .AsNoTracking()
                                .Include(e => e.Campaign)
                                .ThenInclude(e => e.Images)
                                .Where(e => e.Id == i.OfferId)
                                .Select(e => new OfferResponse
                                {
                                    Id = e.Id,
                                    CampaignId = e.CampaignId,
                                    PricingModel = e.PricingModel,
                                    Description = e.Description,
                                    StepInfo = e.StepInfo,
                                    StartDate = e.StartDate,
                                    EndDate = e.EndDate,
                                    Bid = e.Bid,
                                    Budget = e.Budget,
                                    CommissionRate = e.CommissionRate,
                                    OrderReturnTime = e.OrderReturnTime,
                                    ImageUrl = e.ImageUrl,
                                    Status = e.Status.ToString(),
                                    PubOfferStatus = (int)i.Status,
                                    Campaign = new CampaignDetailedResponse
                                    {
                                        Id = e.Campaign.Id,
                                        Name = e.Campaign.Name,
                                        Description = e.Campaign.Description,
                                        StartDate = e.Campaign.StartDate,
                                        EndDate = e.Campaign.EndDate,
                                        ProductUrl = e.Campaign.ProductUrl,
                                        TrackingParams = e.Campaign.TrackingParams,
                                        CategoryId = e.Campaign.CategoryId,
                                        CategoryName = e.Campaign.Name,
                                        Status = e.Campaign.Status.ToString(),
                                        CampImages = e.Campaign.Images!.Select(e => e.ImageUrl!).ToList()
                                    }
                                }).FirstOrDefaultAsync();
                if(offer is not null)
                {
                    responses.Add(offer);
                }
            }
            return responses;
        }

        public async Task<bool> UpdateOfferStatus(long offerId, string status, string? rejectReason)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                var offerRepository = _unitOfWork.GetRepository<Offer>();
                var offer = await offerRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.Id ==  offerId);
                if (offer is null) throw new KeyNotFoundException("Offer does not exists.");

                if (!Enum.TryParse<OfferStatus>(status, true, out var offerStatus))
                    throw new ArgumentException("Invalid offer's status. Please check again!");

                var campaignRepository = _unitOfWork.GetRepository<Campaign>();
                var campaign = await campaignRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.Id == offer.CampaignId);

                if (campaign is null) throw new KeyNotFoundException("Campaign does not exists.");

                if (campaign.Status == CampaignStatus.Rejected)
                    throw new ArgumentException("Campaign must be pending or verified to update offer's status");

                if(offerStatus == OfferStatus.Ended) throw new ArgumentException("This status can only changed automatically!");

                if (offerStatus == OfferStatus.Pending)
                {
                    if(offer.Status != OfferStatus.Rejected) throw new ArgumentException("Offer must be rejected to return to pending");
                    offer.Status = offerStatus;
                    offer.RejectedReason = string.Empty;
                }

                if (offerStatus == OfferStatus.Rejected) {
                    if(offer.Status != OfferStatus.Pending) throw new ArgumentException("Offer must be pending to be rejected");
                    offer.RejectedReason = rejectReason;
                    offer.Status = offerStatus;
                }

                if (offerStatus == OfferStatus.Approved)
                {
                    if (offer.Status != OfferStatus.Pending) throw new ArgumentException("Offer status must be pending to be approved");

                    if (campaign.Status == CampaignStatus.Ended || campaign.Status == CampaignStatus.Rejected)
                        throw new ArgumentException("This campaign's offer can't be approved");

                    if(campaign.Status == CampaignStatus.Pending)
                    {
                        campaign.Status = CampaignStatus.Verified;
                    }
                     
                    offer.Status = offerStatus;
                    offer.RejectedReason = string.Empty;
                }

                if(offerStatus == OfferStatus.Started)
                {
                    if (offer.Status != OfferStatus.Approved) throw new ArgumentException("Offer status must be approved to start");

                    if (campaign.Status == CampaignStatus.Ended || campaign.Status == CampaignStatus.Rejected)
                        throw new ArgumentException("This campaign's offer can't be started");

                    if(campaign.Status == CampaignStatus.Pending)
                    {
                        campaign.Status = CampaignStatus.Started;
                    }

                    offer.Status = offerStatus;
                    offer.RejectedReason = string.Empty;
                }

                campaignRepository.Update(campaign);
                offerRepository.Update(offer);

                var user = await userRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.UserCode == campaign.AdvertiserCode);
                if (user is null) throw new KeyNotFoundException("Advertiser does not exist!");

                var message = new EmailMessage
                {
                    To = user.Email,
                    Subject = "Campaign notifications"
                };

                var emailResult = await _emailService.SendCampaignNotificationEmail(message, campaign.Name, campaign.Id, offer.Id, campaign.Status.ToString());
                if (emailResult)
                {
                    await _notificationService.NotifyOfferStatus(user.UserCode, offer.Id, offer.Status.ToString()!, offer.RejectedReason);
                    return await _unitOfWork.SaveAsync() > 0;
                }
                else
                {
                    throw new Exception("Failed to send email for campaign notifications!");
                }
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();  
                throw;
            }
        }
    }
}
