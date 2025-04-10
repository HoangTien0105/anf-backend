namespace ANF.Core.Models.Responses
{
    public class AdvertiserOfferStatsResponse
    {
        public long Id { get; set; }

        public long OfferId { get; set; }

        /// The date of the statistics.
        public DateTime Date { get; set; }

        public int ClickCount { get; set; }

        public int ConversionCount { get; set; }

        /// Conversion rate = ConversionCount / ClickCount
        public decimal ConversionRate { get; set; }

        /// The number of publisher joined to an offer.
        public int PublisherCount { get; set; }
        public decimal Revenue { get; set; }
    }
}
