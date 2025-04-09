namespace ANF.Core.Models.Responses
{
    public class PublisherOfferStatsResponse
    {
        public long Id { get; set; }
        public long OfferId { get; set; }
        public string? PublisherCode { get; set; }
        public DateTime Date { get; set; }
        public int ClickCount { get; set; }

        /// The number of success tracking for the offer.
        public int ConversionCount { get; set; }

        /// Conversion rate = ConversionCount / ClickCount
        public decimal ConversionRate { get; set; }

        public decimal Revenue { get; set; }
    }
}
