using ANF.Core.Commons;
using ANF.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Information about a subscription
    /// </summary>
    public class Subscription : IEntity
    {
        /// <summary>
        /// Id of the subscription, not auto incremment
        /// </summary>
        [Column("sub_id")]
        public long Id { get; set; }

        [Column("sub_name")]
        public string Name { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }

        [Column("price_per_month", TypeName = "decimal(12)")]
        public decimal? PricePerMonth { get; set; }

        [Column("price_per_year", TypeName = "decimal(12)")]
        public decimal? PricePerYear { get; set; }

        /// <summary>
        /// Short description of benefit when advertisers choose yearly billing
        /// </summary>
        [Column("pricing_benefit")]
        public string? PricingBenefit { get; set; }
        
        [Column("max_created_campaign")]
        public int MaxCreatedCampaign { get; set; }

        [Column("priority_level")]
        public CampaignDisplayPriority? PriorityLevel { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}
