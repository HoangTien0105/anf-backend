using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Statistics of a campaign for advertiser.
    /// </summary>
    public class AdvertiserCampaignStats : IEntity
    {
        [Column("id")]
        public long Id { get; set; }
        
        [Column("campaign_id")]
        public long CampaignId { get; set; }

        /// <summary>
        /// The date of the statistics.
        /// </summary>
        [Column("date")]
        public DateTime Date { get; set; }

        [Column("total_click")]
        public int TotalClick { get; set; }

        [Column("total_verified_click")]
        public int TotalVerifiedClick { get; set; }

        [Column("total_fraud_click")]
        public int TotalFraudClick { get; set; }

        [Column("total_joined_publisher")]
        public int TotalJoinedPublisher { get; set; }

        [Column("total_rejected_publisher")]
        public int TotalRejectedPublisher { get; set; }

        [Column("total_budget_spent", TypeName ="decimal(12)")]
        public decimal TotalBudgetSpent { get; set; }
        
        [Column("total_mobile")]
        public int TotalMobile { get; set; }

        [Column("total_computer")]
        public int TotalComputer { get; set; }

        [Column("total_tablet")]
        public int TotalTablet { get; set; }
    }
}
