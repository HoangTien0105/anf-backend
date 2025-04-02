namespace ANF.Core.Models.Responses
{
    public class CampaignResponse
    {
        public long Id { get; set; }
        public string AdvertiserCode { get; set; } = null!;
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Balance { get; set; }

        public string ProductUrl { get; set; } = null!;

        public string? TrackingParams { get; set; }
        public string? RejectReason { get; set; }

        public long? CategoryId { get; set; }
        public string? CategoryName { get; init; }

        public string Status { get; set; } = null!;
        public ICollection<string> CampImages { get; set; } = new List<string>();
        //public CategoryResponse? Category { get; set; }
        public ICollection<OfferResponse>? Offers { get; set; }

        public ICollection<CampaignImageResponse>? Images { get; set; }
    }
}
