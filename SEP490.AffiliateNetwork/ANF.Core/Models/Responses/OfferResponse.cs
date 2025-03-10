using ANF.Core.Models.Entities;


namespace ANF.Core.Models.Responses
{
    public class OfferResponse
    {
        public long Id { get; set; }

        public long CampaignId { get; set; }

        public string? PricingModel { get; set; }

        public string? Note { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<CampaignImage>? Images { get; set; }
    }
}
