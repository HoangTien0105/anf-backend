using ANF.Core;
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
    public class CampaignService(IUnitOfWork unitOfWork,
                                 ICloudinaryService cloudinaryService,
                                 IMapper mapper,
                                 IUserClaimsService userClaimsService) : ICampaignService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IUserClaimsService _userClaimsService = userClaimsService;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;

        public async Task<bool> CreateCampaign(CampaignCreateRequest request)
        {
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            var offerRepository = _unitOfWork.GetRepository<Offer>();
            var userRepository = _unitOfWork.GetRepository<User>();
            var categoryRepository = _unitOfWork.GetRepository<Category>();
            var imageRepository = _unitOfWork.GetRepository<CampaignImage>();

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

                if (request.CategoryId.HasValue)
                {
                    var category = await categoryRepository.GetAll()
                                .AsNoTracking()
                                .Where(e => e.Id == request.CategoryId)
                                .FirstOrDefaultAsync();
                    if (category is null) throw new KeyNotFoundException("Category does not exists");

                }

                if (request.StartDate <= DateTime.UtcNow.AddDays(1))
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

                campaignRepository.Add(campaign);

                var offersRequest = _mapper.Map<List<OfferCreateRequest>>(request.Offers);

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

                    var offerData = _mapper.Map<Offer>(offer);

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
                return affectedRows > 0;
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

        public async Task<PaginationResponse<CampaignResponse>> GetCampaigns(PaginationRequest request)
        {
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            var campaigns = await campaignRepository.GetAll()
                            .AsNoTracking()
                            .Include(e => e.Images)
                            .Include(e => e.Category)
                            .Include(e => e.Offers)
                            .Where(e => e.Status == CampaignStatus.Verified)
                            .Skip((request.pageNumber - 1) * request.pageSize)
                            .Take(request.pageSize)
                            .ToListAsync();
            if (!campaigns.Any())
                throw new KeyNotFoundException("No data for campaigns!");
            var totalCounts = campaigns.Count();

            var data = _mapper.Map<List<CampaignResponse>>(campaigns);
            return new PaginationResponse<CampaignResponse>(data, totalCounts, request.pageNumber, request.pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id">Advertiser's code</param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<PaginationResponse<CampaignResponse>> 
            GetCampaignsByAdvertisersWithOffers(PaginationRequest request, string id)
        {
            var currentAdvertiserCode = _userClaimsService.GetClaim(ClaimConstants.NameId);
            if (id != currentAdvertiserCode)
                throw new UnauthorizedAccessException("Advertiser's code does not match!");
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            var offerRepository = _unitOfWork.GetRepository<Offer>();
            var campaigns = await campaignRepository.GetAll()
                            .AsNoTracking()
                            .Where(e => e.AdvertiserCode.ToString() == id)  
                            .Include(e => e.Category)
                            .Include(e => e.Images)
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

            var totalCounts = campaigns.Count();

            return new PaginationResponse<CampaignResponse>(data, totalCounts, request.pageNumber, request.pageSize);
        }

        public async Task<PaginationResponse<CampaignResponse>> GetCampaignsWithOffers(PaginationRequest request)
        {
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            var offerRepository = _unitOfWork.GetRepository<Offer>();
            var campaigns = await campaignRepository.GetAll()
                            .AsNoTracking()
                            .Include(e => e.Category)
                            .Include(e => e.Images)
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

            var totalCounts = data.Count();

            return new PaginationResponse<CampaignResponse>(data, totalCounts, request.pageNumber, request.pageSize);
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

                if (request.StartDate <= DateTime.UtcNow.AddDays(1))
                    throw new ArgumentException("Campaign start date must be after today atleast 1");

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

                // Need to review update image function
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

                if(!Enum.TryParse<CampaignStatus>(campaignStatus, true, out var status))
                    throw new ArgumentException("Invalid campaign's status. Please check again!");

                var campaign = await campaignRepository.GetAll()
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(e => e.Id == id);
                if(campaign is null)
                    throw new KeyNotFoundException("Campaign does not exist!");

                if (status != CampaignStatus.Rejected)
                    rejectReason = String.Empty;

                campaign.Status = status;
                campaign.RejectReason = rejectReason;

                campaignRepository.Update(campaign);
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
