using ANF.Core.Models.Responses;

namespace ANF.Core.Services
{
    public interface IStatisticService
    {
        //PublisherOffer
        Task<List<PublisherOfferStatsResponse>> GetAllPublisherOfferStatsByPublisherCode(string publisherCode);
        Task<PublisherOfferStatsResponse> GetPublisherOfferStatsByOfferId(long offerId, string publisherCode);
        Task<bool> generatePublisherOfferStatsByOfferId(long offerId, string publisherCode);
        Task<bool> generatePublisherOfferStatsByPublisherCode(string publisherCode);

        //AdvertiserOffer
        Task<List<AdvertiserOfferStatsResponse>> GetAllAdvertiserOffersStatsByAdvertiserCode(string advertiserCode);
        Task<AdvertiserOfferStatsResponse> GetAdvertiserOfferStatsByOfferId(long offerId);
        Task<bool> generateAdvertiserOfferStatsByOfferId(long offerId);
        Task<bool> generateAdvertiserOfferStatsByAdvertiserCode(string advertiserCode);
    }
}
