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
    public class OfferService(IUnitOfWork unitOfWork, IMapper mapper) : IOfferService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<bool> CreateOffer(OfferCreateRequest request)
        {
            try
            {
                var offerRepository = _unitOfWork.GetRepository<Offer>();
                var campaignRepository = _unitOfWork.GetRepository<Campaign>();

                if (request is null) throw new NullReferenceException("Invalid request data. Please check again!");
                var duplicatedOffer = await offerRepository.GetAll()
                                        .AsNoTracking()
                                        .AnyAsync(e => e.CampaignId == request.CampaignId &&
                                        e.StartDate == request.StartDate &&
                                        e.EndDate == request.EndDate &&
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

                if (request.StartDate < campaignExist.StartDate && request.StartDate > campaignExist.EndDate)
                    throw new ArgumentOutOfRangeException("Offer start date must be between start and end date of campaign");

                if (request.EndDate < campaignExist.StartDate && request.EndDate > campaignExist.EndDate)
                    throw new ArgumentOutOfRangeException("Offer end date must be between start and end date of campaign");

                if (request.EndDate <= request.StartDate)
                    throw new ArgumentOutOfRangeException("End date must be after start date");

                var offer = _mapper.Map<Offer>(request);
                var duplicatedOfferId = await offerRepository.GetAll()
                                        .AsNoTracking()
                                        .AnyAsync(e => e.Id == offer.Id);
                if (duplicatedOfferId) throw new DuplicatedException("Something went wrong. Please try to submit again!");
                offerRepository.Add(offer);
                var affectedRows = await _unitOfWork.SaveAsync();
                return affectedRows > 0;
            }
            catch (Exception)
            {
                //await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteOffer(long id)
        {
            //try
            //{
            //    var offerRepository = _unitOfWork.GetRepository<Offer>();
            //    var imageRepository = _unitOfWork.GetRepository<Image>();
            //    var campaignRepository = _unitOfWork.GetRepository<Campaign>();
            //    var offer = await offerRepository.GetAll()
            //        .AsNoTracking()
            //        .FirstOrDefaultAsync(u => u.Id == id);
            //    if (offer is not null)
            //    {
            //        var campaign = await campaignRepository.GetAll()
            //                .AsNoTracking()
            //                .Where(e => e.Id == offer.CampaignId)
            //                .FirstOrDefaultAsync();
            //        if (campaign is not null)
            //        {
            //            if (campaign.Status != CampaignStatus.Pending)
            //            {
            //                throw new InvalidOperationException("Campaign status must be Pending to delete offer");
            //            }
            //            else
            //            {

            //                var imageUrl = await imageRepository.GetAll()
            //                        .AsNoTracking()
            //                        .Where(u => u.OfferId == offer.Id)
            //                        .ToListAsync();

            //                if (imageUrl.Any())
            //                {
            //                    imageRepository.DeleteRange(imageUrl);
            //                }


            //                offerRepository.Delete(offer);
            //                return await _unitOfWork.SaveAsync() > 0;
            //            }

            //        }
            //        else
            //        {
            //            throw new KeyNotFoundException("Campaign does not exists");
            //        }
            //    }
            //    else
            //    {
            //        throw new KeyNotFoundException("Offer does not exist!");
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            throw new NotImplementedException();
        }

        public async Task<OfferResponse> GetOffer(long offerId)
        {
            var offerRepository = _unitOfWork.GetRepository<Offer>();
            var offer = await offerRepository.FindByIdAsync(offerId);
            if (offer is null)
                throw new KeyNotFoundException("Offer does not exist!");
            var response = _mapper.Map<OfferResponse>(offer);
            return response;
        }

        public async Task<PaginationResponse<OfferResponse>> GetOffers(PaginationRequest request)
        {
            /*var offerRepository = _unitOfWork.GetRepository<Offer>();
            var offers = await offerRepository.GetAll()
                            .AsNoTracking()
                            .Include(e => e.Images)
                            .Skip((request.pageNumber - 1) * request.pageSize)
                            .Take(request.pageSize)
                            .ToListAsync();
            if (!offers.Any())
                throw new KeyNotFoundException("No data for offers!");
            var totalCounts = offers.Count();

            var data = _mapper.Map<List<OfferResponse>>(offers);
            return new PaginationResponse<OfferResponse>(data, totalCounts, request.pageNumber, request.pageSize);*/
            throw new NotImplementedException();
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

                _ = _mapper.Map(request, offer);
                offerRepository.Update(offer);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception)
            {
                //await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
