using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Handling fraud data from tracking event table
    /// </summary>
    public class FraudDetection : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("fraud_id")]
        public long Id { get; set; }

        /// <summary>
        /// Unique, one-to-one relationship with tracking event
        /// </summary>
        [Column("click_id")]
        public string? ClickId { get; set; }

        [Column("publisher_code")]
        public string PublisherCode { get; set; } = null!;

        [Column("reason")]
        public string? Reason { get; set; }

        [Column("detected_time")]
        public DateTime DetectedTime { get; set; }

        public TrackingEvent? TrackingEvent { get; set; }
    }
}
