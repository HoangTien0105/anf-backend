using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class WithdrawalRequest
    {
        [Required(ErrorMessage = "Amount is required!")]
        [Range(2001, double.MaxValue, ErrorMessage = "The amount to be deposited into the wallet must be 2.000 VND or more")]
        public decimal Amount { get; set; }
        
        [Required(ErrorMessage = "Banking number is required!", AllowEmptyStrings = false)]
        public string BankingNo { get; set; } = null!;

        /// <summary>
        /// Beneficiary bank code retrieved from the Techcombank template
        /// </summary>
        [Required(ErrorMessage = "Beneficiary bank code is required!", AllowEmptyStrings = false)]
        public string BeneficiaryBankCode { get; set; } = null!;

        /// <summary>
        /// Beneficiary bank name retrieved from the Techcombank template
        /// </summary>
        [Required(ErrorMessage = "Beneficiary bank name is required!", AllowEmptyStrings = false)]
        public string BeneficiaryBankName { get; set; } = null!;
    }
}
