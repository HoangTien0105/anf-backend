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

namespace ANF.Service
{
    public class CampaignService(IUnitOfWork unitOfWork,
                                 IMapper mapper) : ICampaignService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
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
