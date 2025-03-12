using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;

namespace ANF.Core.Services
{
    public interface IAdvertiserService
    {
        Task<bool> AddProfile(long advertiserId, AdvertiserProfileRequest profile);

        Task<AdvertiserProfileResponse> GetAdvertiserProfile(long advertiserId);

        Task<bool> UpdateProfile(long advertiserId, AdvertiserProfileUpdatedRequest request);

        Task<bool> AddBankingInformation(Guid advertiserCode, List<UserBankCreateRequest> requests);

        Task<bool> UpdateBankingInformation(long userBankId, UserBankUpdateRequest request);

        /// <summary>
        /// Remove user bank accounts
        /// </summary>
        /// <param name="ubIds">List user bank's id</param>
        /// <returns></returns>
        Task<bool> DeleteBankingInformation(List<long> ubIds);
    }
}
