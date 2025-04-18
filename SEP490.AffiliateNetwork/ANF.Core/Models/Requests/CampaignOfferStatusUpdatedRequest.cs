using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    /// <summary>
    /// Model for updating campaign and offer status
    /// </summary>
    public class CampaignOfferStatusUpdatedRequest
    {
        [Required(ErrorMessage = "Status is required!", AllowEmptyStrings = false)]
        public string Status { get; set; } = null!;

        public string? RejectReason { get; set; }
    }
}
