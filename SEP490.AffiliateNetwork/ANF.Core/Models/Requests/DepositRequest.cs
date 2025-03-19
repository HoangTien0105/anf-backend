using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class DepositRequest
    {
        [Required(ErrorMessage = "Amount is required")]
        [Range(2000, 100000000, ErrorMessage = "Amount must be greater than or equal to 2.000")]
        public decimal Amount { get; set; } = 2000;
    }
}
