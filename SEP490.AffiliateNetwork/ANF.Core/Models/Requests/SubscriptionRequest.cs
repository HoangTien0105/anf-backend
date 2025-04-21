using System.ComponentModel.DataAnnotations;
namespace ANF.Core.Models.Requests
{
    public class SubscriptionRequest
    {
        [Required(ErrorMessage = "Name is required.", AllowEmptyStrings = false)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.", 
            AllowEmptyStrings = false)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Price per month is required.")]
        [Range(1000, double.MaxValue, ErrorMessage = "Price must be greater or equal to 1000 VND.")]
        public decimal PricePerMonth { get; set; }

        [Required(ErrorMessage = "Price per year is required.")]
        [Range(1000, double.MaxValue, ErrorMessage = "Price must be greater or equal to 1000 VND.")]
        public decimal PricePerYear { get; set; }

        public string? PricingBenefit { get; set; }

        [Required(ErrorMessage = "Number of created campaign is required!")]
        [Range(1, 80, ErrorMessage = "Number of created campaign is between 1 and 80 campaigns")]
        public int MaxCreatedCampaign { get; set; }
    }
}
