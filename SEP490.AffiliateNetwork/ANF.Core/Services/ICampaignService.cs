using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
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
    }
}
