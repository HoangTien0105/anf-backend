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

        Task<bool> AddProfile(long publisherId, PublisherProfileRequest value);

        Task<PublisherResponse> GetPublisherProfile(long publisherId);

        Task<List<AffiliateSourceResponse>> GetAffiliateSourceOfPublisher(long publisherId);
    }
}
