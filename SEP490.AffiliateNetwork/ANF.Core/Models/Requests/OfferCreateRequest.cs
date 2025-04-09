using ANF.Core.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class OfferCreateRequest
    {
        [BindProperty(Name = "campaignId")]
        [Required(ErrorMessage = "Campaign id is required.")]
        public long CampaignId { get; set; }

        [BindProperty(Name = "pricingModel")]
        [Required(ErrorMessage = "Pricing model is required.", AllowEmptyStrings = false)]
        public string PricingModel { get; set; } = null!;

        [BindProperty(Name = "description")]
        [Required(ErrorMessage = "Description is required.", AllowEmptyStrings = false)]
        public string Description { get; set; } = null!;

        [BindProperty(Name = "stepInfo")]
        [Required(ErrorMessage = "StepInfo is required.", AllowEmptyStrings = false)]
        public string StepInfo { get; set; } = null!;

        [BindProperty(Name = "startDate")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [BindProperty(Name = "endDate")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [BindProperty(Name = "bid")]
        [Required(ErrorMessage = "Bid is required")]
        [Range(300, 500000, ErrorMessage = "Bid must be at least 300 VND and maximum at 500.000 VND")]
        public decimal Bid { get; set; }

        [BindProperty(Name = "budget")]
        [Required(ErrorMessage = "Budget is required")]
        [Range(1000, double.MaxValue, ErrorMessage = "Budget must be at least 1000 VND and greater than the Bid")]
        public decimal Budget { get; set; }
        
        [BindProperty(Name = "commissionRate")]
        public double? CommissionRate { get; set; }

        [BindProperty(Name = "orderReturnTime")]
        [Range(0, 15, ErrorMessage = "Order return date must not exceed 15 days")]
        public int? OrderReturnTime { get; set; }

        [BindProperty(Name = "offerImages")]
        [AllowedImageExtensions]
        public IFormFile? OfferImages { get; set; }
    }
}
