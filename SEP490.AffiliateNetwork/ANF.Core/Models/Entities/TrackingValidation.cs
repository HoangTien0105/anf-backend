using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Handling valid tracking data
    /// </summary>
    public class TrackingValidation : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("validation_id")]
        public long Id { get; set; }

        /// <summary>
        /// Unique click's id, one-to-one relationship with TrackingEvent
        /// </summary>
        [Column("click_id")]
        public long ClickId { get; set; }

        [Column("offer_id")]
        public long OfferId { get; set; }

        [Column("validated_time")]
        public DateTime ValidatedTime { get; set; }

        [Column("click_date")]
        public DateOnly ClickDate { get; set; }

        //TODO: Define status of conversion
        [Column("conversion_status")]
        public string? ConversionStatus { get; set; }

        [Column("revenue")]
        public double? Revenue { get; set; }
    }
}
