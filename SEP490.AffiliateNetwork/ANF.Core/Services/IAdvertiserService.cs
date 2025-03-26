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
    }
}
