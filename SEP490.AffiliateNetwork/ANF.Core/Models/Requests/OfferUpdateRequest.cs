using ANF.Core.Validations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class OfferUpdateRequest
    {
        [Required(ErrorMessage = "Pricing model is required.", AllowEmptyStrings = false)]
        public string PricingModel { get; set; } = null!;

        [Required(ErrorMessage = "Description model is required.", AllowEmptyStrings = false)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "StepInfo is required.", AllowEmptyStrings = false)]
        public string StepInfo { get; set; } = null!;

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Bid is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Bid is not negative")]
        public decimal Bid { get; set; }

        [Required(ErrorMessage = "Budget is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Budget is not negative")]
        public decimal Budget { get; set; }

        [AllowedImageExtensions]
        public IFormFile? OfferImages { get; set; }
    }
}
