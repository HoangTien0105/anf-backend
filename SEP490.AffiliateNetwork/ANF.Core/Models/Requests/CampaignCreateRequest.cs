using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class CampaignCreateRequest
    {
        [BindProperty(Name = "advertiserCode")]
        [Required(ErrorMessage = "AdvertiserCode is required.")]
        public string AdvertiserCode { get; set; } = null!;

        [BindProperty(Name = "name")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;

        [BindProperty(Name = "description")]
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = null!;

        [BindProperty(Name = "startDate")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [BindProperty(Name = "endDate")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [BindProperty(Name = "productUrl")]
        [Required(ErrorMessage = "ProductUrl is required.", AllowEmptyStrings = false)]
        public string ProductUrl { get; set; } = null!;

        [BindProperty(Name = "trackingParams")]
        [Required(ErrorMessage = "TrackingParams is required.")]
        public string TrackingParams { get; set; } = null!;

        [BindProperty(Name = "categoryId")]
        public long? CategoryId { get; set; }

        [BindProperty(Name = "offers")]
        [Required(ErrorMessage = "Offer is required.")]
        public List<OfferForCampaignCreateRequest> Offers { get; set; } = new List<OfferForCampaignCreateRequest>();

        [BindProperty(Name = "imgFiles")]
        [Required(ErrorMessage = "Images is required.")]
        public List<IFormFile> ImgFiles { get; set; } = new List<IFormFile>();
    }

    public class OfferForCampaignCreateRequest
    {
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
        [Range(1000, double.MaxValue, ErrorMessage = "Bid is not negative")]
        public decimal Bid { get; set; }

        [BindProperty(Name = "budget")]
        [Required(ErrorMessage = "Budget is required")]
        [Range(1000, double.MaxValue, ErrorMessage = "Budget is not negative")]
        public decimal Budget { get; set; }

    }
}
