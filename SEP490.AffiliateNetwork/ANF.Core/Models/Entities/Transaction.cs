using ANF.Core.Commons;
using ANF.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    public class Transaction : IEntity
    {
        [Column("trans_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("user_code")]
        public Guid UserCode { get; set; }

        [Column("wallet_id")]
        public long WalletId { get; set; }

        [Column("amount")]
        public double Amount { get; set; }

        public long? CampaignId { get; set; }

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
