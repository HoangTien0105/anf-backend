using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Represents the statistics of an advertiser's offer.
    /// </summary>
    [Table("advertiser_offer_stats")]
    public class AdvertiserOfferStats : IEntity
    {
        [Column("id")]
        public long Id { get; set; }
        
        [Column("offer_id")]
        public long OfferId { get; set; }

        /// <summary>
        /// The date of the statistics.
        /// </summary>
        [Column("date")]
        public DateTime Date { get; set; }

        [Column("click_count")]
        public int ClickCount { get; set; }

        [Column("conversion_count")]
        public int ConversionCount { get; set; }

        /// <summary>
        /// Conversion rate = ConversionCount / ClickCount
        /// </summary>
        [Column("conversion_rate", TypeName = "decimal(12)")]
        public decimal ConversionRate { get; set; }

        /// <summary>
        /// The number of publisher joined to an offer.
        /// </summary>
        [Column("publisher_count")]
        public int PublisherCount { get; set; }

        [Column("revenue", TypeName = "decimal(12)")]
        public decimal Revenue { get; set; }
    }
}
