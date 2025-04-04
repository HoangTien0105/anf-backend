namespace ANF.Core.Commons
{
    /// <summary>
    /// Tracking validate event
    /// </summary>
    public class TrackingConversionEvent
    {
        public long Id { get; set; }
        public string ClickId { get; set; } = null!;
        public string PublisherCode { get; set; } = null!;
        public long OfferId { get; set; }
        public string PricingModel { get; set; } = null!;
        public decimal? Amount { get; set; }
    }
}
