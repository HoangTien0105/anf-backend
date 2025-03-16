using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class UserBankUpdateRequest
    {
        [Required(ErrorMessage = "Banking number is required!")]
        [Range(1, double.MaxValue, ErrorMessage = "Banking number must be a non-negative value.")]
        public long BankingNo { get; set; }

        [Required(ErrorMessage = "Banking provider is required!", AllowEmptyStrings = false)]
        public string BankingProvider { get; set; } = null!;
    }
}
