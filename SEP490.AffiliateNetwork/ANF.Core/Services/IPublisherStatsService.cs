using ANF.Core.Models.Responses;

namespace ANF.Core.Services
{
    public interface IPublisherStatsService
    {
        Task<List<PublisherStatsResponse>> GetRevenueStatsByCampaignId(long campaignId, DateTime from, DateTime to);
        Task<List<PublisherStatsResponse>> GetRevenueStats(DateTime from, DateTime to);
        Task<bool> GeneratePublisherOfferStatsByPublisherCode(DateTime startDate, DateTime endDate);
    }
}
