using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class UserBankUpdatedRequest
    {
        public string AccountName { get; set; } = null!;

        [Required(ErrorMessage = "Banking number is required!")]
        [Range(1, double.MaxValue, ErrorMessage = "Banking number must be a non-negative value.")]
        public string BankingNo { get; set; } = null!;

        [Required(ErrorMessage = "Banking code is required!", AllowEmptyStrings = false)]
        public string BankingCode { get; set; } = null!;

        [Required(ErrorMessage = "Banking provider is required!", AllowEmptyStrings = false)]
        public string BankingName { get; set; } = null!;
    }
}
