namespace ANF.Core.Models.Responses
{
    /// <summary>
    /// Withdrawal response for admin to approve or reject
    /// </summary>
    public class WithdrawalResponse
    {
        /// <summary>
        /// Transaction's id
        /// </summary>
        public long Id { get; set; }

        public string UserCode { get; set; } = null!;

        public long WalletId { get; set; }

        public decimal Amount { get; set; }

        public string? Reason { get; set; }

        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Current bank account of the user to withdraw the money 
        /// </summary>
        public string? CurrentBankingNo { get; set; }
    }
}
