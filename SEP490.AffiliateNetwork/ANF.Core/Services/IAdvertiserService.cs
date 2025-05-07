using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;

namespace ANF.Core.Services
{
    public interface IAdvertiserService
    {
        Task<bool> AddProfile(long advertiserId, AdvertiserProfileRequest profile);

        Task<AdvertiserProfileResponse> GetAdvertiserProfile(long advertiserId);

        Task<bool> UpdateProfile(long advertiserId, AdvertiserProfileUpdatedRequest request);

        Task<List<AffiliateSourceResponse>> GetTrafficSourceOfPublisher(long publisherId);

        Task<List<PublisherInformationForAdvertiser>> GetPendingPublisherInOffer(string offerId);

        /// <summary>
        /// Get click statistics for a campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        Task<List<AdvertiserCampaignStatsResponse.ClickStats>> GetClickStatistics(long campaignId,
            DateTime from,
            DateTime to);

        /// <summary>
        /// Get device statistics for a campaign
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns> <summary>
        Task<List<AdvertiserCampaignStatsResponse.DeviceStats>> GetDeviceStatistics(long campaignId,
            DateTime from,
            DateTime to);

        Task<List<AdvertiserCampaignStatsResponse.OfferStats>> GetOfferStatistics(long campaignId,
            DateTime from,
            DateTime to);
    }
}
