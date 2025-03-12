using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class CampaignImgCreateRequest
    {
        public long? CampaignId { get; set; }
        [Required(ErrorMessage = "ImageUrl is required.")]
        public string ImageUrl { get; set; } = null!;
    }
}
