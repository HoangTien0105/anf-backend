using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class UserBankCreateRequest
    {
        [Required(ErrorMessage = "Banking number is required!")]
        public long BankingNo { get; set; }

        [Required(ErrorMessage = "Banking provider is required!")]
        public string BankingProvider { get; set; } = null!;
    }
}
