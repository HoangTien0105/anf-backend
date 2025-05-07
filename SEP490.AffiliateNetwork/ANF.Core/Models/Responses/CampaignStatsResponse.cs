namespace ANF.Core.Models.Responses
{
    /// <summary>
    /// Response model for campaign statistics (total clicks, joined publishers, verified clicks)
    /// </summary>
    public class CampaignStatsResponse
    {
        public long CampaignId { get; set; }
        public string CampaignName { get; set; } = null!;
        public int TotalClicks { get; set; }
        public int TotalJoinedPublishers { get; set; }
        public int TotalVerifiedClicks { get; set; }
        public DateTime Date { get; set; }
    }
}