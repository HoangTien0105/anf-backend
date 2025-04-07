using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class PostbackRequest
    {
        [Required(ErrorMessage = "Click id is required.")]
        public string ClickId { get; set; } = null!;

        [Required(ErrorMessage = "Amount is required")]
        [Range(1000, double.MaxValue, ErrorMessage = "Amount must be at least 1000 VND")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = null!;
        public string? TransactionId { get; set; }
    }
}
