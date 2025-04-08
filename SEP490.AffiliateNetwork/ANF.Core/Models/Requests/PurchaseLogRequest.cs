using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class PurchaseLogRequest
    {
        [Required(ErrorMessage = "Click id is required.")]
        public string ClickId { get; set; } = null!;

        [Required(ErrorMessage = "Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a non-negative number.")]
        public double Amount { get; set; }

        public string? ItemName { get; set; }

        public int? Quantity { get; set; }

        public string? TransactionId { get; set; }
    }
}
