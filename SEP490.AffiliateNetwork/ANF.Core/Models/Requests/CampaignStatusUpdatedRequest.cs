using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    /// <summary>
    /// Model for updating campaign status
    /// </summary>
    public class CampaignStatusUpdatedRequest
    {
        [Required(ErrorMessage = "Status is required!", AllowEmptyStrings = false)]
        public string CampaignStatus { get; set; } = null!;

        public string? RejectReason { get; set; }
    }
}
