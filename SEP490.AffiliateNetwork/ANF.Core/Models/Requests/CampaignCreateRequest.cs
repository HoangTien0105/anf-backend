using ANF.Core.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANF.Core.Models.Requests
{
    public class CampaignCreateRequest
    {
        [Required(ErrorMessage = "AdvertiserId is required.")]
        public long AdvertiserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "ProductUrl is required.", AllowEmptyStrings = false)]
        public string ProductUrl { get; set; } = null!;

        [Required(ErrorMessage = "TrackingParams is required.")]
        public string TrackingParams { get; set; } = null!;

        [Required(ErrorMessage = "CategoryId is required.")]
        public long CategoryId { get; set; }

        [Required(ErrorMessage = "Offer is required.")]
        public List<OfferForCampaignCreateRequest> Offers { get; set; } = new List<OfferForCampaignCreateRequest>();

        [Required(ErrorMessage = "Images is required.")]
        public List<IFormFile> ImgFiles { get; set; } = new List<IFormFile>();
    }

    public class OfferForCampaignCreateRequest
    {
        [Required(ErrorMessage = "Pricing model is required.", AllowEmptyStrings = false)]
        public string PricingModel { get; set; } = null!;
        public string? Note { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

    }
}
