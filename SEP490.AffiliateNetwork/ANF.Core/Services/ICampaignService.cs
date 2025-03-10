using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANF.Core.Services
{
    public interface ICampaignService
    {
        Task<PaginationResponse<CampaignResponse>> GetCampaigns(PaginationRequest request);
        Task<PaginationResponse<CampaignResponse>> GetCampaignsWithOffers(PaginationRequest request);
        Task<PaginationResponse<CampaignResponse>> GetCampaignsByAdvertisersWithOffers(PaginationRequest request, long id);
        Task<bool> CreateCampaign(CampaignCreateRequest request);
    }
}
