using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class CampaignUpdateRequest
    {
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
            
        [BindProperty(Name = "imgFiles")]
        [Required(ErrorMessage = "Images is required.")]
        public List<IFormFile> ImgFiles { get; set; } = new List<IFormFile>();
    }
}
