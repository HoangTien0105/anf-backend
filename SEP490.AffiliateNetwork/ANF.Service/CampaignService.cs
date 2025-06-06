﻿using ANF.Core;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ANF.Core.Enums;
using ANF.Core.Commons;
using ANF.Core.Exceptions;

namespace ANF.Service
{
    public class CampaignService(IUnitOfWork unitOfWork,
                                 ICloudinaryService cloudinaryService,
                                 IMapper mapper,
                                 IUserClaimsService userClaimsService,
                                 INotificationService notificationService,
                                 IEmailService emailService) : ICampaignService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IUserClaimsService _userClaimsService = userClaimsService;
        private readonly INotificationService _notificationService = notificationService;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly IEmailService _emailService = emailService;

        public async Task<bool> CreateCampaign(CampaignCreateRequest request)
        {
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            var offerRepository = _unitOfWork.GetRepository<Offer>();
            var userRepository = _unitOfWork.GetRepository<User>();
            var categoryRepository = _unitOfWork.GetRepository<Category>();
            var imageRepository = _unitOfWork.GetRepository<CampaignImage>();
            var walletRepository = _unitOfWork.GetRepository<Wallet>();

            try
            {
                var currentAdvertiserCode = _userClaimsService.GetClaim(ClaimConstants.NameId);
                if (request.AdvertiserCode != currentAdvertiserCode)
                {
                    throw new UnauthorizedAccessException("Advertiser's code does not match!");
                }
                if (request is null) throw new NullReferenceException("Invalid request data. Please check again!");

                var advertiser = await userRepository.GetAll()
                                            .AsNoTracking()
                                            .Where(e => e.UserCode.ToString() == request.AdvertiserCode && e.Role == UserRoles.Advertiser)
                                            .FirstOrDefaultAsync();
                if (advertiser is null) throw new KeyNotFoundException("Advertiser does not exists");

                var advWallet = await walletRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.UserCode == request.AdvertiserCode);
                if(advWallet is null) throw new KeyNotFoundException("Advertiser wallet does not exists");

                if (request.CategoryId.HasValue)
                {
                    var category = await categoryRepository.GetAll()
                                .AsNoTracking()
                                .Where(e => e.Id == request.CategoryId)
                                .FirstOrDefaultAsync();
                    if (category is null) throw new KeyNotFoundException("Category does not exists");

                }

                if (request.StartDate <= DateTime.Now.AddDays(1))
                    throw new ArgumentException("Campaign start date must be after today at least 1 day");

                if (request.EndDate <= request.StartDate)
                    throw new ArgumentException("End date must be after start date");

                if (!Uri.TryCreate(request.ProductUrl, UriKind.Absolute, out var uriResult) ||
                    (uriResult is not null && uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                    throw new ArgumentException("ProductUrl must be a valid URL with http or https scheme (e.g., http://example.com or https://example.com).");

                if (request.ProductUrl.EndsWith("/"))
                    throw new ArgumentException("ProductUrl must not end with a trailing slash (/).");

                if (!request.Offers.Any())
                    throw new InvalidOperationException("At least 1 offer required");

                if (!request.ImgFiles.Any())
                    throw new InvalidOperationException("At least 1 images required");

                var campaign = _mapper.Map<Campaign>(request);

                campaign.Balance = request.Offers.Sum(e => e.Budget);

                if (campaign.Balance > advWallet.Balance)
                    throw new ArgumentException("Advertiser does not have enough for this campaign.");

                var advCampaignMoney = await campaignRepository.GetAll()
                    .AsNoTracking()
                    .Where(e => e.AdvertiserCode == request.AdvertiserCode
                    && (e.Status == CampaignStatus.Started
                    ||  e.Status == CampaignStatus.Verified
                    ||  e.Status == CampaignStatus.Pending))
                    .SumAsync(e => e.Balance);

                if(advCampaignMoney + campaign.Balance > advWallet.Balance)
                    throw new ArgumentException("Advertiser does not have enough balance to cover all campaigns.");

                campaign.Budget = campaign.Balance;

                campaignRepository.Add(campaign);

                var offersRequest = _mapper.Map<List<OfferCreateRequest>>(request.Offers);

                //Offer kh trùng
                var isDuplicateOffer = offersRequest.GroupBy(o =>
                                        new { o.PricingModel, o.Description, o.StepInfo, o.StartDate, o.EndDate }).Any(g => g.Count() > 1);
                if (isDuplicateOffer)
                    throw new InvalidOperationException("Offers can't be the same");

                foreach (var offer in offersRequest)
                {
                    offer.CampaignId = campaign.Id;

                    var validModel = PricingModelConstant.pricingModels.Any(e => e.Name.Trim() == offer.PricingModel.Trim());
                    if (!validModel) throw new KeyNotFoundException("Pricing model does not exists");

                    if (offer.StartDate < campaign.StartDate || offer.StartDate > campaign.EndDate)
                        throw new ArgumentException("Offer start date must be between start and end date of campaign");

                    if (offer.EndDate < campaign.StartDate || offer.EndDate > campaign.EndDate)
                        throw new ArgumentException("Offer end date must be between start and end date of campaign");

                    if (offer.EndDate <= offer.StartDate)
                        throw new ArgumentException("Offer end date must be after offer start date");

                    if (offer.Bid >= offer.Budget)
                        throw new ArgumentException("Offer bid can't be higher than offer budget");

                    if (offer.CommissionRate is not null)
                    {
                        if (offer.CommissionRate <= 0) throw new ArgumentException("Commission rate must be greater than 0");
                    }

                    if (offer.PricingModel == "CPS" && offer.CommissionRate is null) throw new ArgumentException("Pricing model CPS must have Commission rate");

                    var offerData = _mapper.Map<Offer>(offer);

                    offerData.CommissionRate = offer.CommissionRate;

                    if(offer.OrderReturnTime is not null)
                    {
                        offerData.OrderReturnTime = offer.OrderReturnTime + " days";    
                    }

                    offerRepository.Add(offerData);
                }

                var allowedImagesTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };

                foreach (var image in request.ImgFiles)
                {
                    if (!allowedImagesTypes.Contains(image.ContentType))
                    {
                        throw new ArgumentException($"File {image.FileName} is not an allowed image format! Only JPEG, PNG, GIF, and WebP are supported.");
                    }

                    var imageUrl = await _cloudinaryService.UploadImageAsync(image);
                    if (imageUrl is not null)
                    {
                        CampaignImgCreateRequest imageCreateRequest = new CampaignImgCreateRequest
                        {
                            CampaignId = campaign.Id,
                            ImageUrl = imageUrl,
                        };

                        var imageData = _mapper.Map<CampaignImage>(imageCreateRequest);
                        imageRepository.Add(imageData);
                    }
                }

                var affectedRows = await _unitOfWork.SaveAsync();
                if(affectedRows > 0)
                {
                    await _notificationService.NotifyCampaignCreated("A new campaign has just created", campaign.Id);
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

        public async Task<bool> DeleteCampaign(long id)
        {
            try
            {
                var offerRepository = _unitOfWork.GetRepository<Offer>();
                var imageRepository = _unitOfWork.GetRepository<CampaignImage>();
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();
                var campaign = await campaignRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (campaign is not null)
                {
                    if (campaign.Status != CampaignStatus.Pending)
                    {
                        throw new InvalidOperationException("Campaign status must be Pending to delete campaign");
                    }

                    var offers = await offerRepository.GetAll()
                            .AsNoTracking()
                            .Where(e => e.CampaignId == campaign.Id)
                            .ToListAsync();

                    if (offers.Any())
                    {
                        offerRepository.DeleteRange(offers);
                    }
                    var imageUrl = await imageRepository.GetAll()
                            .AsNoTracking()
                            .Where(u => u.CampaignId == campaign.Id)
                            .ToListAsync();

                    if (imageUrl.Any())
                    {
                        imageRepository.DeleteRange(imageUrl);
                    }

                    campaignRepository.Delete(campaign);
                    return await _unitOfWork.SaveAsync() > 0;
                }
                else
                {
                    throw new KeyNotFoundException("Campaign does not exist!");
                }
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<CampaignDetailedResponse> GetCampaign(long id)
        {
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            var campaign = await campaignRepository.GetAll()
                .AsNoTracking()
                .Include(c => c.Images)
                .Include(c => c.Offers)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (campaign is null)
            {
                throw new KeyNotFoundException("Campaign does not exist!");
            }
            return _mapper.Map<CampaignDetailedResponse>(campaign);
        }

        public async Task<CampaignPubDetailedResponse> GetCampaignForPublisher(long id)
        {
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            var pubOfferRepository = _unitOfWork.GetRepository<PublisherOffer>();
            var userRepository = _unitOfWork.GetRepository<User>();
            var publisherCode = _userClaimsService.GetClaim(ClaimConstants.NameId);

            var publisher = await userRepository.GetAll()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.UserCode == publisherCode);

            if (publisher is null) throw new KeyNotFoundException("Publisher does not exist!");
            if (publisher.Role != UserRoles.Publisher)
                throw new ForbiddenException("This user does not have access permission.");

            var campaign = await campaignRepository.GetAll()
               .AsNoTracking()
               .Include(c => c.Images)
               .Include(c => c.Offers)
               .Include(c => c.Category)
               .FirstOrDefaultAsync(c => c.Id == id &&
               (c.Status == CampaignStatus.Verified || c.Status == CampaignStatus.Started));

            if (campaign is null)
            {
                throw new KeyNotFoundException("Campaign does not exist!");
            }

            var data = _mapper.Map<CampaignPubDetailedResponse>(campaign);

            foreach (var item in data.Offers)
            {
                var pubOffer = await pubOfferRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.OfferId == item.Id && e.PublisherCode == publisherCode);
                if (pubOffer is null)
                {
                    item.PubOfferStatus = 0;
                }
                else
                {
                    item.PubOfferStatus = (int)pubOffer.Status;
                }
            }

            return data;
        }

        public async Task<PaginationResponse<CampaignDetailedResponse>> GetCampaigns(PaginationRequest request, long? cateId)
        {
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            var query = campaignRepository.GetAll()
                .AsNoTracking()
                .Include(e => e.Images)
                .Include(e => e.Category)
                .Include(e => e.Offers)
                .Where(e => e.Status == CampaignStatus.Verified || e.Status == CampaignStatus.Started);

            if (cateId.HasValue)
            {
                query = query.Where(e => e.CategoryId == cateId.Value);
            }

            var totalRecord = await query.CountAsync();
            
            var campaigns = await query
                .Skip((request.pageNumber - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();
            if (!campaigns.Any())
                throw new KeyNotFoundException("No data for campaigns!");

            var data = _mapper.Map<List<CampaignDetailedResponse>>(campaigns);
            return new PaginationResponse<CampaignDetailedResponse>(data, totalRecord, request.pageNumber, request.pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request">Pagination request</param>
        /// <param name="id">Advertiser's code</param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<PaginationResponse<CampaignResponse>>
            GetCampaignsByAdvertisersWithOffers(PaginationRequest request, string id, string? search)
        {
            var currentAdvertiserCode = _userClaimsService.GetClaim(ClaimConstants.NameId);
            if (id != currentAdvertiserCode)
                throw new UnauthorizedAccessException("Advertiser's code does not match!");
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            var offerRepository = _unitOfWork.GetRepository<Offer>();

            IQueryable<Campaign> query = campaignRepository.GetAll()
                .AsNoTracking()
                .Where(e => e.AdvertiserCode.ToString() == id)
                .Include(e => e.Category)
                .Include(e => e.Images);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e => e.Name.ToLower().Contains(search.ToLower()));
            }

            var totalRecord = await query.CountAsync();
            var campaigns = await query
                            .Skip((request.pageNumber - 1) * request.pageSize)
                            .Take(request.pageSize)
                            .ToListAsync();
            if (!campaigns.Any())
                throw new KeyNotFoundException("No data for campaigns!");

            List<CampaignResponse> data = _mapper.Map<List<CampaignResponse>>(campaigns);

            foreach (var campaign in data)
            {
                var offers = await offerRepository.GetAll()
                            .AsNoTracking()
                            .Where(e => e.CampaignId == campaign.Id)
                            .ToListAsync();

                campaign.Offers = _mapper.Map<List<OfferResponse>>(offers);
            }
            return new PaginationResponse<CampaignResponse>(data, totalRecord, 
                request.pageNumber, 
                request.pageSize);
        }

        public async Task<List<CampaignDetailedResponse>> GetCampaignsWithDateRange(DateTime from, DateTime to)
        {
            if (from > to) throw new ArgumentException("To date must be after from date");

            var currentUserCode = _userClaimsService.GetClaim(ClaimConstants.NameId);
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            var userRepository = _unitOfWork.GetRepository<User>();
            var pubOfferRepository = _unitOfWork.GetRepository<PublisherOffer>();

            var user = await userRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.UserCode == currentUserCode);

            IQueryable<Campaign> query;

            if (user!.Role == UserRoles.Advertiser)
            {
                query = campaignRepository.GetAll()
                                          .AsNoTracking()
                                          .Where(e => e.AdvertiserCode == currentUserCode
                                              && e.StartDate <= to && e.EndDate >= from);
            }  
            else if(user.Role == UserRoles.Publisher)
            {
                var joinOffers = await pubOfferRepository.GetAll()
                        .AsNoTracking()
                        .Where(e => e.PublisherCode == currentUserCode)
                        .Select(e => e.OfferId)
                        .ToListAsync();

                query = campaignRepository.GetAll()
                            .AsNoTracking()
                            .Where(e => e.Offers.Any(o => joinOffers.Contains(o.Id))
                                     && e.StartDate <= to && e.EndDate >= from);
            }
            else
            {
                throw new UnauthorizedAccessException("User role is not authorized to access this function.");
            }

            var totalRecord = await query.CountAsync();
            var campaigns = await query
                            .Include(c => c.Images)
                            .Include(c => c.Category)
                            .Include(c => c.Offers)
                            .OrderByDescending(c => c.StartDate)
                            .ToListAsync();

            if (!campaigns.Any())
            {
                throw new KeyNotFoundException("No data for campaigns!");
            }

            var data = _mapper.Map<List<CampaignDetailedResponse>>(campaigns);
            return data;
        }

        public async Task<PaginationResponse<CampaignResponse>> GetCampaignsWithOffers(PaginationRequest request, string? search)
        {
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            var offerRepository = _unitOfWork.GetRepository<Offer>();

            IQueryable<Campaign> query;

            query = campaignRepository.GetAll()
                            .AsNoTracking()
                            .Include(e => e.Category)
                            .Include(e => e.Images);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e => e.Name.Contains(search));
            }

            var totalRecord = await query.CountAsync();
            var campaigns = await query
                            .Skip((request.pageNumber - 1) * request.pageSize)
                            .Take(request.pageSize)
                            .ToListAsync();
            if (!campaigns.Any())
                throw new KeyNotFoundException("No data for campaigns!");

            List<CampaignResponse> data = _mapper.Map<List<CampaignResponse>>(campaigns);

            foreach (var campaign in data)
            {
                var offers = await offerRepository.GetAll()
                             .AsNoTracking()
                             .Where(e => e.CampaignId == campaign.Id)
                             .ToListAsync();

                campaign.Offers = _mapper.Map<List<OfferResponse>>(offers);
            }
            return new PaginationResponse<CampaignResponse>(data, totalRecord, 
                request.pageNumber, 
                request.pageSize);
        }

        public async Task<bool> UpdateCampaignInformation(long id, CampaignUpdateRequest request)
        {
            try
            {
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();
                var categoryRepository = _unitOfWork.GetRepository<Category>();
                var imageRepository = _unitOfWork.GetRepository<CampaignImage>();

                if (request is null)
                    throw new ArgumentException("Invalid data request. Please check again!");
                var campaign = await campaignRepository.GetAll()
                                .AsNoTracking()
                                .FirstOrDefaultAsync(e => e.Id == id);

                if (campaign is null)
                    throw new KeyNotFoundException("Campaign does not exist!");

                if (campaign.Status != CampaignStatus.Pending)
                    throw new ArgumentException("Can't update this campaign");

                if (request.CategoryId.HasValue)
                {
                    var category = await categoryRepository.GetAll()
                                .AsNoTracking()
                                .Where(e => e.Id == request.CategoryId)
                                .FirstOrDefaultAsync();
                    if (category is null) throw new KeyNotFoundException("Category does not exists");

                }

                if (request.StartDate <= DateTime.Now.AddDays(1))
                    throw new ArgumentException("Campaign start date must be after today at least 1 day");

                if (request.EndDate <= request.StartDate)
                    throw new ArgumentException("End date must be after start date");

                if (!Uri.TryCreate(request.ProductUrl, UriKind.Absolute, out var uriResult) ||
                    (uriResult is not null && uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                    throw new ArgumentException("ProductUrl must be a valid URL with http or https scheme (e.g., http://example.com or https://example.com).");

                if (request.ProductUrl.EndsWith("/"))
                    throw new ArgumentException("ProductUrl must not end with a trailing slash (/).");

                _ = _mapper.Map(request, campaign);
                campaignRepository.Update(campaign);

                var allowedImagesTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };

                foreach (var image in request.ImgFiles)
                {
                    if (!allowedImagesTypes.Contains(image.ContentType))
                    {
                        throw new ArgumentException($"File {image.FileName} is not an allowed image format! Only JPEG, PNG, GIF, and WebP are supported.");
                    }

                    var imageUrl = await _cloudinaryService.UploadImageAsync(image);
                    if (imageUrl is not null)
                    {
                        CampaignImgCreateRequest imageCreateRequest = new CampaignImgCreateRequest
                        {
                            CampaignId = campaign.Id,
                            ImageUrl = imageUrl,
                        };

                        var imageData = _mapper.Map<CampaignImage>(imageCreateRequest);
                        imageRepository.Add(imageData);
                    }
                }

                return await _unitOfWork.SaveAsync() > 0;

            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateCampaignStatus(long id, string campaignStatus, string? rejectReason)
        {
            try
            {
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();
                var offerRepository = _unitOfWork.GetRepository<Offer>();
                var userRepository = _unitOfWork.GetRepository<User>();

                if (!Enum.TryParse<CampaignStatus>(campaignStatus, true, out var status))
                    throw new ArgumentException("Invalid campaign's status. Please check again!");

                var campaign = await campaignRepository.GetAll()
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(e => e.Id == id);
                if (campaign is null)
                    throw new KeyNotFoundException("Campaign does not exist!");

                if (status == CampaignStatus.Rejected)
                {
                    campaign.RejectReason = rejectReason;
                }

                var offers = await offerRepository.GetAll().AsNoTracking().Where(e => e.CampaignId == id).ToListAsync();

                if (offers.Count > 0)
                {
                    switch (status)
                    {
                        case CampaignStatus.Verified:
                            {
                                if (campaign.Status != CampaignStatus.Pending)
                                    throw new ArgumentException("Campaign must be pending to be applied");
                                offers.ForEach(offer => offer.Status = OfferStatus.Approved);
                                break;
                            }
                        case CampaignStatus.Started:
                            {
                                if (campaign.Status != CampaignStatus.Verified)
                                    throw new ArgumentException("Campaign must be verified to start");
                                if (campaign.StartDate < DateTime.Now) throw new ArgumentException("The start date has not been reached yet.");
                                offers.ForEach(offer => offer.Status = OfferStatus.Started);
                                break;
                            }
                        case CampaignStatus.Rejected:
                            {
                                if (campaign.Status != CampaignStatus.Pending)
                                    throw new ArgumentException("Campaign can't be rejected anymore");
                                offers.ForEach(offer => offer.Status = OfferStatus.Rejected);
                                break;
                            }
                        case CampaignStatus.Pending:
                            {
                                if (campaign.Status != CampaignStatus.Rejected)
                                    throw new ArgumentException("Campaign must be rejected to be pending again");
                                offers.ForEach(offer => offer.Status = OfferStatus.Pending);
                                break;
                            }

                        case CampaignStatus.Ended:
                            {
                                throw new ArgumentException("Campaign can't be end manually!");
                            }
                        default:
                            break;
                    }

                    campaign.Status = status;
                    offerRepository.UpdateRange(offers);
                }
                campaignRepository.Update(campaign);

                var user = await userRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(e => e.UserCode == campaign.AdvertiserCode);
                if(user is null) throw new KeyNotFoundException("Advertiser does not exist!");

                var message = new EmailMessage
                {
                    To = user.Email,
                    Subject = "Campaign notifications"
                };

                var emailResult = await _emailService.SendCampaignNotificationEmail(message, campaign.Name, campaign.Id, null, campaign.Status.ToString());
                await _notificationService.NotifyCampaignStatus(user.UserCode, campaign.Id, campaign.Status.ToString(), campaign.RejectReason);
                if (emailResult)
                {
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
