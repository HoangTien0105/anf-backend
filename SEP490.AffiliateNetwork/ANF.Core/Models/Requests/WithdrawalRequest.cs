using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class WithdrawalRequest
    {
        [Required(ErrorMessage = "Amount is required!")]
        [Range(2001, double.MaxValue, ErrorMessage = "The amount to be deposited into the wallet must be 2.000 VND or more")]
        public decimal Amount { get; set; }
        
        [Required(ErrorMessage = "Withdrawal reason is required!", AllowEmptyStrings = false)]
        public string Reason { get; set; } = "Rút tiền từ ví về tài khoản đã chọn"; // Default message

        [Required(ErrorMessage = "Banking number is required!", AllowEmptyStrings = false)]
        public string BankingNo { get; set; } = null!;

        [Required(ErrorMessage = "Beneficiary bank code is required!", AllowEmptyStrings = false)]
        public string BeneficiaryBankCode { get; set; } = null!;

        [Required(ErrorMessage = "Beneficiary bank name is required!", AllowEmptyStrings = false)]
        public string BeneficiaryBankName { get; set; } = null!;
    }
}
