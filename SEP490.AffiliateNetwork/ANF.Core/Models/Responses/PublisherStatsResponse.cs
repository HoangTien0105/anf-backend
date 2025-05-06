namespace ANF.Core.Models.Responses
{
    public class PublisherStatsResponse
    {
        public DateTime Date { get; set; }
        public List<CampaignStatsDto> Campaigns { get; set; } = new List<CampaignStatsDto>();
    }

    public class CampaignStatsDto
    {
        public long CampaignId { get; set; }
        public decimal TotalRevenue { get; set; }
        //public decimal TotalClick { get; set; }
        //public decimal TotalVerifiedClick { get; set; }
        //public decimal TotalFraudClick { get; set; }
        //public decimal TotalTablet { get; set; }
        //public decimal TotalMobile { get; set; }
        //public decimal TotalComputer { get; set; }
    }
}
