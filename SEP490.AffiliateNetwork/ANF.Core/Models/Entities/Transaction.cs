using ANF.Core.Commons;
using ANF.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Table to store all transactions occured in the platform
    /// </summary>
    [Table("Transactions")]
    public class Transaction : IEntity
    {
        [Column("trans_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("user_code")]
        public Guid UserCode { get; set; }

        [Column("wallet_id")]
        public long WalletId { get; set; }

        [Column("amount", TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        [Column("campaign_id")]
        public long? CampaignId { get; set; }

        [Column("subscription_id")]
        public long? SubscriptionId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Payment status: Success, Failed, Canceled
        /// </summary>
        [Column("payment_status")]
        public PaymentStatus Status { get; set; }

        public WalletHistory? WalletHistory { get; set; }

        public Wallet? Wallet { get; set; }

        public User? User { get; set; }

        public Campaign? Campaign { get; set; }

        public Subscription? Subscription { get; set; }
    }
}
