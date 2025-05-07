using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;

namespace ANF.Core.Services
{
    public interface ICampaignService
    {
        Task<PaginationResponse<CampaignDetailedResponse>> GetCampaigns(PaginationRequest request, long? cateId);
        Task<List<CampaignDetailedResponse>> GetCampaignsWithDateRange(DateTime from, DateTime to);
        Task<PaginationResponse<CampaignResponse>> GetCampaignsWithOffers(PaginationRequest request, string? search);
        Task<PaginationResponse<CampaignResponse>> GetCampaignsByAdvertisersWithOffers(PaginationRequest request, string id, string? search);
        Task<bool> CreateCampaign(CampaignCreateRequest request);
        Task<bool> UpdateCampaignInformation(long id, CampaignUpdateRequest request);
        Task<bool> UpdateCampaignStatus(long id, string campaignStatus, string? rejectReason);
        Task<bool> DeleteCampaign(long id);

        /// <summary>
        /// Get a campaign
        /// </summary>
        /// <param name="id">Campaign's id</param>
        /// <returns>Campaign and offer information</returns>
        Task<CampaignDetailedResponse> GetCampaign(long id);

        /// <summary>
        /// Get a campaign for publisher
        /// </summary>
        /// <param name="id">Campaign's id</param>
        /// <returns>Campaign and offer information for publisher</returns>
        Task<CampaignPubDetailedResponse> GetCampaignForPublisher(long id);
    }
}
