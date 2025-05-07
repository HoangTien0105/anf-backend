namespace ANF.Core.Models.Responses
{
    public class CampaignStatsAdminResponse
    {
        public DateTime Date { get; set; }
        public int TotalCampaign { get; set; }

        public int TotalApprovedCampaign { get; set; }
        
        public int TotalRejectedCampaign { get; set; }
    }
}
