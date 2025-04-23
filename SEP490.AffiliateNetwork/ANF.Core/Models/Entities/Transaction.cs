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

        /// <summary>
        /// The time created the transaction
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Approved date of requesting to withdraw the money from users
        /// </summary>
        [Column("approved_at")]
        public DateTime? ApprovedAt { get; set; }

        /// <summary>
        /// Banking number selected by user to receive the money
        /// </summary>
        [Column("current_banking_no")]
        public string? CurrentBankingNo { get; set; }

        /// <summary>
        /// Indicate the transaction is withdrawal
        /// </summary>
        [Column("is_withdrawal")]
        public bool IsWithdrawal { get; set; } = false;

        /// <summary>
        /// Billing type of subscription
        /// </summary>
        [Column("billing_type")]
        public SubscriptionBillingType? BillingType { get; set; }

        /// <summary>
        /// The start date and time when the subscription becomes valid or effective.
        /// </summary>
        [Column("valid_from")]
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// The end date and time when the subscription is no longer valid or effective.
        /// </summary>
        [Column("valid_to")]
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// The remained slot to create campaigns in a subscription 
        /// </summary>
        [Column("remained_slot")]
        public int? RemainedSlot { get; set; }

        [Column("status")]
        public TransactionStatus Status { get; set; }

        public WalletHistory? WalletHistory { get; set; }

        public Wallet? Wallet { get; set; }

        public User? User { get; set; }

        public Campaign? Campaign { get; set; }

        public Subscription? Subscription { get; set; }
    }
}
