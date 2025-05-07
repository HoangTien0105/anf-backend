namespace ANF.Core.Models.Responses
{
    public class AdvertiserCampaignStatsResponse
    {
        public DateTime Date { get; set; }

        public List<CampaignClickStats> CampaignClickStats { get; set; } = [];
        
        public List<OfferStats> OfferStats { get; set; } = [];
        
        public List<DeviceStats> DeviceStats { get; set; } = [];
    }

    public class CampaignClickStats
    {
        public long CampaignId { get; set; }

        public int? TotalClick { get; set; }

        public int? TotalValidClick { get; set; }

        public int? TotalFraudClick { get; set; }
    }

    public class OfferStats
    {
        public long CampaignId { get; set; }

        public int? TotalOffer { get; set; }

        public int? TotalJoinedPublisher { get; set; }

        public int? TotalRejectedPublisher { get; set; }
    }

    public class DeviceStats
    {
        public long CampaignId { get; set; }

        public int? TotalMobile { get; set; }

        public int? TotalComputer { get; set; }

        public int? TotalTablet { get; set; }
    }
}