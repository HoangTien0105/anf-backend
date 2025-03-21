using ANF.Core.Commons;
using ANF.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Data collected from the customer by platform
    /// </summary>
    public class TrackingEvent
    {
        [Column("click_id")]
        public string Id { get; set; } = null!;

        [Column("offer_id")]
        public long OfferId { get; set; }

        [Column("publisher_code")]
        public string PublisherCode { get; set; } = null!;

        [Column("ip_address")]
        public string? IpAddress { get; set; }

        [Column("user_agent", TypeName = "text")]
        public string? UserAgent { get; set; }

        [Column("site_id")]
        public string? SiteId { get; set; }

        [Column("country")]
        public string? Country { get; set; }

        [Column("carrier")]
        public string? Carrier { get; set; }

        [Column("click_time")]
        public DateTime ClickTime { get; set; }

        [Column("referer")]
        public string? Referer { get; set; }

        [Column("proxy")]
        public string? Proxy { get; set; }

        [Column("status")]
        public TrackingEventStatus? Status { get; set; }

        public Offer? Offer { get; set; }

        public FraudDetection? FraudDetection { get; set; }

        public TrackingValidation? TrackingValidation { get; set; }
    }
}
