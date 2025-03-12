using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    public class WalletHistory : IEntity
    {
        [Column("wh_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("amount")]
        public double Amount { get; set; }

        /// <summary>
        /// The time used to logging any changes are made in the wallet
        /// </summary>
        [Column("changed_time")]
        public DateTime ChangedTime { get; set; }

        /// <summary>
        /// Current wallet balance
        /// </summary>
        [Column("wallet_balance")]
        public double WalletBalance { get; set; }

        /// <summary>
        /// Type of transaction - DEPOSIT (TRANSACTION), WITHDRAW, OFFER FEE, SUBSCRIPTION
        /// </summary>
        [Column("type")]
        public string? Type { get; set; }

        [Column("transaction_id")]
        public long? TransactionId { get; set; }

        [Column("campaign_id")]
        public long? CampaignId { get; set; }

        [Column("subscription_id")]
        public long? SubscriptionId { get; set; }

        [Column("wallet_id")]
        public long WalletId { get; set; }

        public Wallet? Wallet { get; set; }

        public Subscription? Subscription { get; set; }

        public Campaign? Campaign { get; set; }

        public PaymentTransaction? PaymentTransaction { get; set; }
    }
}
