namespace ANF.Core.Models.Responses
{
    public class AdvertiserCampaignStatsResponse
    {
        public class ClickStats
        {
            public int? TotalClick { get; set; }

            public int? TotalValidClick { get; set; }

            public int? TotalFraudClick { get; set; }
        }

        public class OfferStats
        {
            public int? TotalOffer { get; set; }

            public int? TotalJoinedPublisher { get; set; }

            public int? TotalRejectedPublisher { get; set; }
        }

        public class DeviceStats
        {
            public int? TotalMobile { get; set; }

            public int? TotalComputer { get; set; }

            public int? TotalTablet { get; set; }
        }
    }
}