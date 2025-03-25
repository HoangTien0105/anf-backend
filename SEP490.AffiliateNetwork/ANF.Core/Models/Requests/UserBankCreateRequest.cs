using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class UserBankCreateRequest
    {
        [Required(ErrorMessage = "Account's name is required!", AllowEmptyStrings = false)]
        public string AccountName { get; set; } = null!;

        [Required(ErrorMessage = "Banking number is required!")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Banking number must contain only numbers.")]
        public string BankingNo { get; set; } = null!;

        public string BankingCode { get; set; } = null!;

        [Required(ErrorMessage = "Banking name is required!", AllowEmptyStrings = false)]
        public string BankingName { get; set; } = null!;
    }
}
