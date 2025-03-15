using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class CampaignUpdateRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = null!;

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
        public long? CategoryId { get; set; }
        [Required(ErrorMessage = "Images is required.")]
        public List<IFormFile> ImgFiles { get; set; } = new List<IFormFile>();
    }
}
