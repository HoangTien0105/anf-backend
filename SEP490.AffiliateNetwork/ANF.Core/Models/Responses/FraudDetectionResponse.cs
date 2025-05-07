namespace ANF.Core.Models.Responses
{
    public class FraudDetectionResponse
    {
        public long Id { get; set; }
        public string? ClickId { get; set; }
        public string? Reason { get; set; }
        public DateTime DetectedTime { get; set; }
        public long? OfferId {  get; set; }
    }
}
