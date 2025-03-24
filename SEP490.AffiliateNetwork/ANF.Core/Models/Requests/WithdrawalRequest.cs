using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class WithdrawalRequest
    {
        [Required(ErrorMessage = "Amount is required!")]
        [Range(2001, double.MaxValue, ErrorMessage = "The amount to be deposited into the wallet must be 2.000 VND or more")]
        public decimal Amount { get; set; }
        
        [Required(ErrorMessage = "Withdrawal reason is required!", AllowEmptyStrings = false)]
        public string Reason { get; set; } = null!;
    }
}
