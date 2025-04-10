using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class PostbackLogUpdateRequest
    {
        [RegularExpression(@"^\S*$", ErrorMessage = "TransactionId must not contain whitespace.")]
        public string? TransactionId { get; set; }
        [RegularExpression(@"^\S*$", ErrorMessage = "ItemName must not contain whitespace.")]
        public string? ItemName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Quantity can't be negative")]
        public int? Quantity { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Amount can't be negative")]
        public decimal Amount { get; set; }
    }
}
