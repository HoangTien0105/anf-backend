using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class AffiliateSourceUpdateRequest
    {
        [Required(ErrorMessage = "Provider is required!", AllowEmptyStrings = false)]
        public string? Provider { get; set; }

        [Required(ErrorMessage = "Source's url is required!", AllowEmptyStrings = false)]
        public string SourceUrl { get; set; } = null!;

        public string? Type { get; set; }
    }
}
