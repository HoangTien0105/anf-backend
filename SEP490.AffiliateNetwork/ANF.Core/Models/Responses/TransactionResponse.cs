using ANF.Core.Enums;

namespace ANF.Core.Models.Responses
{
    /// <summary>
    /// Response model for transaction (Detailed transaction)
    /// </summary>
    public class TransactionResponse
    {
        public long Id { get; set; }

        public string UserCode { get; set; } = null!;

        public long WalletId { get; set; }

        public decimal Amount { get; set; }

        public long? CampaignId { get; set; }

        public long? SubscriptionId { get; set; }

        public string? Reason { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public string? CurrentBankingNo { get; set; }

        public bool IsWithdrawal { get; set; }

        public SubscriptionBillingType? BillingType { get; set; }

        public DateTime? ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public int? RemainedSlot { get; set; }

        public TransactionStatus Status { get; set; }
    }
}
