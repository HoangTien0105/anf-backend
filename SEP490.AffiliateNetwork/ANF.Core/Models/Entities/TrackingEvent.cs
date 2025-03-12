using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Data collected from the customer by platform
    /// </summary>
    public class TrackingEvent : IGuidEntity
    {
        [Column("click_id")]
        public Guid Id { get; set; }

        [Column("offer_id")]
        public long OfferId { get; set; }

        //TODO: Review the field. Use publisher_id or publisher_code?
        [Column("publisher_id")]
        public long PublisherId { get; set; }

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

        //TODO: Define the status of tracking
        [Column("status")]
        public string? Status { get; set; }
    }
}
