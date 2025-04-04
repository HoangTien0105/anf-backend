using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Represents the statistics of an advertiser's campaign.
    /// </summary>
    [Table("advertiser_stats")]
    public class AdvertiserStats : IEntity
    {
        public long Id { get; set; }

        [Column("campaign_id")]
        public long CampaignId { get; set; }

        [Column("offer_id")]
        public long OfferId { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("click_count")]
        public int ClickCount { get; set; }

        [Column("conversion_count")]
        public int ConversionCount { get; set; }
            
        /// <summary>
        /// Conversion rate = ConversionCount / ClickCount
        /// </summary>
        [Column("conversion_rate")]
        public decimal ConversionRate { get; set; }

        [Column("postback_success")]
        public int PostbackSuccess { get; set; }

        [Column("postback_failed")]
        public int PostbackFailed { get; set; }

        [Column("validation_success")]
        public int ValidationSuccess { get; set; }

        [Column("validation_failed")]
        public int ValidationFailed { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
