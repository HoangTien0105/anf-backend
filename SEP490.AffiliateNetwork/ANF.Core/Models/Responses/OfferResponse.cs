namespace ANF.Core.Models.Responses
{
    public class OfferResponse
    {
        public long Id { get; set; }
        public long CampaignId { get; set; }
        public string? PricingModel { get; set; }
        public string Description { get; set; } = null!;
        public string StepInfo { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Bid { get; set; }
        public decimal Budget { get; set; }
        public double? CommissionRate { get; set; }
        public string? OrderReturnTime { get; set; }
        public string? ImageUrl { get; set; }
        public string? Status { get; set; }
        public string? RejectedReason { get; set; }
        public int PubOfferStatus { get; set; }
        public CampaignDetailedResponse? Campaign { get; set; } 
    }
}
