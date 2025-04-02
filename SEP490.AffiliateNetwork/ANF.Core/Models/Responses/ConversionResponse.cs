namespace ANF.Core.Models.Responses
{
    /// <summary>
    /// Model to get the stored data from queue (RabbitMQ)
    /// </summary>
    public class ConversionResponse
    {
        /// <summary>
        /// Validation's id from tracking validation table
        /// </summary>
        public long Id { get; set; }

        public string ClickId { get; set; } = null!;

        public string PublisherCode { get; set; } = null!;

        public long OfferId { get; set; }

        public string PricingModel { get; set; } = null!;

        public int MyProperty { get; set; }

        public decimal Revenue { get; set; }
    }
}
