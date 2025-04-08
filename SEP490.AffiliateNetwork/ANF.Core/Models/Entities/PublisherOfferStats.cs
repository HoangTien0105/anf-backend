using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Represents the statistics of a publisher's offer.
    /// </summary>
    public class PublisherOfferStats : IEntity
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("offer_id")]
        public long OfferId { get; set; }

        [Column("publisher_code")]
        public string? PublisherCode { get; set; }

        /// <summary>
        /// The date of the statistics.
        /// </summary>
        [Column("date")]
        public DateTime Date { get; set; }

        [Column("click_count")]
        public int ClickCount { get; set; }

        /// <summary>
        /// The number of success tracking for the offer.
        /// </summary>
        [Column("conversion_count")]
        public int ConversionCount { get; set; }

        /// <summary>
        /// Conversion rate = ConversionCount / ClickCount
        /// </summary>
        [Column("conversion_rate", TypeName = "decimal(10, 2)")]
        public decimal ConversionRate { get; set; }

        [Column("revenue", TypeName = "decimal(10, 2)")]
        public decimal Revenue { get; set; }
    }
}
