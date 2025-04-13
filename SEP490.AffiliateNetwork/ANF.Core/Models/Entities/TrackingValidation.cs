using ANF.Core.Commons;
using ANF.Core.Enums;
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
        public string? ClickId { get; set; }

        [Column("validated_time")]
        public DateTime ValidatedTime { get; set; }
        
        /// <summary>
        /// Specify the validation of the tracking data
        /// </summary>
        [Column("validation_status")]
        public ValidationStatus? ValidationStatus { get; set; }

        /// <summary>
        /// Specify whether a click's id is converted successfully to the publisher
        /// </summary>
        [Column("conversion_status")]
        public ConversionStatus? ConversionStatus { get; set; }

        [Column("amount", TypeName = "decimal(12)")]
        public decimal? Amount { get; set; }

        public TrackingEvent? TrackingEvent { get; set; }
    }
}
