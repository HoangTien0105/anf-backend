using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;

namespace ANF.Core.Services
{
    public interface IPublisherService
    {
        Task<bool> AddAffiliateSources(long publisherId, List<AffiliateSourceCreateRequest> requests);

        Task<bool> DeleteAffiliateSource(long sourceId);

        Task<bool> DeleteAffiliateSources(List<long> sourceIds);

        Task<bool> UpdateAffiliateSource(long sourceId, AffiliateSourceUpdateRequest request);

        Task<bool> AddProfile(long publisherId, PublisherProfileCreatedRequest value);

        Task<bool> UpdateProfile(long publisherId, PublisherProfileUpdatedRequest request);

        Task<PublisherResponse> GetPublisherProfile(long publisherId);

        Task<List<AffiliateSourceResponse>> GetAffiliateSourceOfPublisher(long publisherId);

        Task<bool> AddBankingInformation(Guid publisherCode, List<UserBankCreateRequest> requests);

        Task<bool> UpdateBankingInformation(long userBankId, UserBankUpdateRequest request);

        /// <summary>
        /// Remove user bank accounts
        /// </summary>
        /// <param name="ubIds">List user bank's id</param>
        /// <returns></returns>
        Task<bool> DeleteBankingInformation(List<long> ubIds);
    }
}
