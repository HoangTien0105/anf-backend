using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;

namespace ANF.Core.Services
{
    public interface IOfferService
    {
        Task<PaginationResponse<OfferResponse>> GetOffers(PaginationRequest request);
        Task<OfferResponse> GetOffer(long offerId);
        Task<bool> CreateOffer(OfferCreateRequest request);
        Task<bool> UpdateOffer(long id, OfferUpdateRequest request);
        Task<bool> DeleteOffer(long id);

        /// <summary>
        /// Aplly offer request for publisher
        /// </summary>
        /// <param name="offerId">Offer's id</param>
        /// <returns></returns>
        Task<bool> ApplyOffer(long offerId);
        Task<bool> UpdateApplyOfferStatus(long pubOfferId, string status, string? rejectReason);

        Task<List<PublisherOfferResponse>> GetPublisherOfOffer(long offerId);
    }
}
