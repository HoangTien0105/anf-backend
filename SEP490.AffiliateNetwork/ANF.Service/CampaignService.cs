using ANF.Core;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using ANF.Infrastructure;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ANF.Core.Enums;
using Microsoft.AspNetCore.Http;
using ANF.Core.Exceptions;

namespace ANF.Service
{
    public class CampaignService(IUnitOfWork unitOfWork,
                                 ICloudinaryService cloudinaryService,
                                 IMapper mapper) : ICampaignService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;

        public async Task<bool> CreateCampaign(CampaignCreateRequest request)
        {
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            var offerRepository = _unitOfWork.GetRepository<Offer>();
            var userRepository = _unitOfWork.GetRepository<User>();
            var categoryRepository = _unitOfWork.GetRepository<Category>();
            var imageRepository = _unitOfWork.GetRepository<Image>();
            try
            {

                if (request is null) throw new NullReferenceException("Invalid request data. Please check again!");
                var advertiser = await userRepository.GetAll()
                                            .AsNoTracking()
                                            .Where(e => e.Id == request.AdvertiserId && e.Role == UserRoles.Advertiser)
                                            .FirstOrDefaultAsync();
                if (advertiser is null) throw new KeyNotFoundException("Advertiser does not exists");

                if (request.CategoryId > 0)
                {
                    var category = await categoryRepository.GetAll()
                                .AsNoTracking()
                                .Where(e => e.Id == request.CategoryId)
                                .FirstOrDefaultAsync();
                    if (category is null) throw new KeyNotFoundException("Category does not exists");

                }

                if (request.StartDate <= DateTime.UtcNow.AddDays(1))
                    throw new ArgumentOutOfRangeException("Campaign start date must be after today atleast 1");

                if (request.EndDate <= request.StartDate)
                    throw new ArgumentOutOfRangeException("End date must be after start date");

                Uri uriResult;
                if (!Uri.TryCreate(request.ProductUrl, UriKind.Absolute, out uriResult) ||
                    (uriResult != null && uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                    throw new ArgumentException("ProductUrl must be a valid URL with http or https scheme (e.g., http://example.com or https://example.com).");

                if (request.ProductUrl.EndsWith("/"))
                    throw new ArgumentException("ProductUrl must not end with a trailing slash (/).");

                if (!request.Offers.Any())
                    throw new InvalidOperationException("At least 1 offer required");

                if (!request.ImgFiles.Any())
                    throw new InvalidOperationException("At least 1 images required");

                var campaign = _mapper.Map<Campaign>(request);

                campaignRepository.Add(campaign);

                var offersRequest = _mapper.Map<List<OfferCreateRequest>>(request.Offers);
                foreach (var offer in offersRequest)
                {
                    offer.CampaignId = campaign.Id;

                    var validModel = PricingModelConstant.pricingModels.Any(e => e.Name.Trim() == offer.PricingModel.Trim());
                    if (!validModel) throw new KeyNotFoundException("Pricing model does not exists");

                    if (offer.StartDate < campaign.StartDate || offer.StartDate > campaign.EndDate)
                        throw new ArgumentOutOfRangeException("Offer start date must be between start and end date of campaign");

                    if (offer.EndDate < campaign.StartDate && offer.EndDate > campaign.EndDate)
                        throw new ArgumentOutOfRangeException("Offer end date must be between start and end date of campaign");

                    if (offer.EndDate <= offer.StartDate)
                        throw new ArgumentOutOfRangeException("End date must be after start date");

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
                    if (imageUrl != null)
                    {
                        ImageCreateRequest imageCreateRequest = new ImageCreateRequest
                        {
                            CampaignId = campaign.Id,
                            ImageUrl = imageUrl,
                        };

                        var imageData = _mapper.Map<Image>(imageCreateRequest);
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

        public async Task<PaginationResponse<CampaignResponse>> GetCampaigns(PaginationRequest request)
        {
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            var campaigns = await campaignRepository.GetAll()
                            .AsNoTracking()
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

        public async Task<PaginationResponse<CampaignResponse>> GetCampaignsByAdvertisersWithOffers(PaginationRequest request, long id)
        {
            var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            var offerRepository = _unitOfWork.GetRepository<Offer>();
            var campaigns = await campaignRepository.GetAll()
                            .AsNoTracking()
                            .Where(e => e.AdvertiserId == id)
                            .Include(e => e.Advertiser)
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
    }
}
