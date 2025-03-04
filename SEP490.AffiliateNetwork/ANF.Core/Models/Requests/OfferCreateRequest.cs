using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class OfferCreateRequest
    {
        [Required(ErrorMessage = "Campaign id is required.")]
        public long CampaignId { get; set; }

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
