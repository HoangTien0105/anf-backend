using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class PostbackRequest
    {
        [Required(ErrorMessage = "Click id is required.")]
        public string ClickId { get; set; } = null!;

        [Required(ErrorMessage = "Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a non-negative number.")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = null!;
        public string? TransactionId { get; set; }
    }
}
