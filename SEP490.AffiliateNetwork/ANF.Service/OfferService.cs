using ANF.Core;
using ANF.Core.Exceptions;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Core.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ANF.Core.Enums;

namespace ANF.Service
{
    public class OfferService(IUnitOfWork unitOfWork, IMapper mapper, ICloudinaryService cloudinaryService) : IOfferService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly IMapper _mapper = mapper;
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
                    throw new ArgumentOutOfRangeException("Offer start date must be between start and end date of campaign");

                if (request.EndDate < campaignExist.StartDate && request.EndDate > campaignExist.EndDate)
                    throw new ArgumentOutOfRangeException("Offer end date must be between start and end date of campaign");

                if (request.EndDate <= request.StartDate)
                    throw new ArgumentOutOfRangeException("End date must be after start date");

                if (request.Bid >= request.Budget)
                {
                    throw new ArgumentOutOfRangeException("Bid can't higher than budget");
                }

                var offer = _mapper.Map<Offer>(request);

                if (request.OfferImages is not null)
                {
                    var allowedImagesTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };

                    if (!allowedImagesTypes.Contains(request.OfferImages.ContentType))
                    {
                        throw new ArgumentException($"File {request.OfferImages.FileName} is not an allowed image format! Only JPEG, PNG, GIF, and WebP are supported.");
                    }

                    var imageUrl = await _cloudinaryService.UploadImageAsync(request.OfferImages);
                    if(imageUrl is not null)
                    {
                        offer.ImageUrl = imageUrl;
                    } else
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
                if(offer is null)
                    throw new KeyNotFoundException("Offer does not exist!");

                var campaign = await campaignRepository.GetAll()
                                        .AsNoTracking()
                                        .Where(e => e.Id == offer.CampaignId)
                                        .Include(e => e.Offers)
                                        .FirstOrDefaultAsync();

                if(campaign is null)
                    throw new KeyNotFoundException("Campaign does not exists");

                if (campaign.Status != CampaignStatus.Pending)
                    throw new InvalidOperationException("Campaign status must be Pending to delete offer");

                if(campaign.Offers.Count < 2)
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
                    throw new ArgumentOutOfRangeException("Offer start date must be between start and end date of campaign");

                if (request.EndDate < campaignExist.StartDate && request.EndDate > campaignExist.EndDate)
                    throw new ArgumentOutOfRangeException("Offer end date must be between start and end date of campaign");

                if (request.EndDate <= request.StartDate)
                    throw new ArgumentOutOfRangeException("End date must be after start date");

                if (request.Bid >= request.Budget)
                {
                    throw new ArgumentOutOfRangeException("Bid can't higher than budget");
                }

                _ = _mapper.Map(request, offer);

                if (request.OfferImages is not null)
                {
                    var allowedImagesTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };

                    if (!allowedImagesTypes.Contains(request.OfferImages.ContentType))
                    {
                        throw new ArgumentException($"File {request.OfferImages.FileName} is not an allowed image format! Only JPEG, PNG, GIF, and WebP are supported.");
                    }

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
