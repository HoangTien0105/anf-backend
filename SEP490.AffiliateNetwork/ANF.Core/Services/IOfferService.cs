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
        Task<bool> ApplyOffer(string pubId, long offerId);
        Task<bool> UpdateApplyOfferStatus(long pubOfferId, string status, string? rejectReason);
    }
}
