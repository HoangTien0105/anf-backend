using ANF.Core.Models.Responses;

namespace ANF.Core.Services
{
    public interface IStatisticService
    {
        //PublisherOffer
        Task<List<PublisherOfferStatsResponse>> GetAllPublisherOfferStatsByPublisherCode(string publisherCode);
        Task<PublisherOfferStatsResponse> GetPublisherOfferStatsByOfferId(long offerId, string publisherCode);
        Task<bool> GeneratePublisherOfferStatsByOfferId(long offerId, string publisherCode);
        Task<bool> GeneratePublisherOfferStatsByPublisherCode(string publisherCode);

        //AdvertiserOffer
        Task<List<AdvertiserOfferStatsResponse>> GetAllAdvertiserOffersStatsByAdvertiserCode(string advertiserCode);
        Task<AdvertiserOfferStatsResponse> GetAdvertiserOfferStatsByOfferId(long offerId);
        Task<bool> GenerateAdvertiserOfferStatsByOfferId(long offerId);
        Task<bool> GenerateAdvertiserOfferStatsByAdvertiserCode(string advertiserCode);
    }
}
