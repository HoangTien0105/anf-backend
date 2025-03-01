using System.ComponentModel.DataAnnotations;
namespace ANF.Core.Models.Requests
{
    public class SubscriptionRequest
    {
        [Required(ErrorMessage = "Name is required.", AllowEmptyStrings = false)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public double Price { get; set; }
        public string? Duration { get; set; }
    }
}
