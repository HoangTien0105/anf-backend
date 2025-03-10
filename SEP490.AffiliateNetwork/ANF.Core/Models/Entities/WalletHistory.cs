using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    // TODO: Add relationship for this entity
    public class WalletHistory
    {
        [Column("wh_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("amount")]
        public double Amount { get; set; }

        /// <summary>
        /// Type of transaction - DEPOSIT (TRANSACTION), WITHDRAW, OFFER FEE, SUBSCRIPTION
        /// </summary>
        [Column("type")]
        public string? Type { get; set; }

        [Column("transaction_id")]
        public long? TransactionId { get; set; }

        [Column("offer_id")]
        public long? OfferId { get; set; }

        [Column("subscription_id")]
        public long? SubscriptionId { get; set; }

        [Column("wallet_id")]
        public long WalletId { get; set; }

        public Wallet? Wallet { get; set; }
    }
}
