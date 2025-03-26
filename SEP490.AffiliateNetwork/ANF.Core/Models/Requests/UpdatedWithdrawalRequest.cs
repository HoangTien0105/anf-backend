using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    /// <summary>
    /// For admin to approve or reject the user's withdrawal request
    /// </summary>
    public class UpdatedWithdrawalRequest
    {
        [Required(ErrorMessage = "Transaction's id is required!")]
        public List<long> TransactionIds { get; set; } = new List<long>();

        [Required(ErrorMessage = "Status is required!", AllowEmptyStrings = false)]
        public string Status { get; set; } = null!;
    }
}
