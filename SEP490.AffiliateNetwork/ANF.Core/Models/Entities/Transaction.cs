using ANF.Core.Commons;
using ANF.Core.Enums;
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
        public long Id { get; set; }

        [Column("user_code")]
        public string UserCode { get; set; } = null!;

        [Column("wallet_id")]
        public long WalletId { get; set; }

        [Column("amount", TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        [Column("campaign_id")]
        public long? CampaignId { get; set; }

        [Column("subscription_id")]
        public long? SubscriptionId { get; set; }

        [Column("reason")]
        public string? Reason { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("approved_at")]
        public DateTime? ApprovedAt { get; set; }

        [Column("current_banking_no")]
        public string? CurrentBankingNo { get; set; }

        [Column("is_withdrawal")]
        public bool IsWithdrawal { get; set; } = false;

        [Column("status")]
        public TransactionStatus Status { get; set; }

        public WalletHistory? WalletHistory { get; set; }

        public Wallet? Wallet { get; set; }

        public User? User { get; set; }

        public Campaign? Campaign { get; set; }

        public Subscription? Subscription { get; set; }
    }
}
