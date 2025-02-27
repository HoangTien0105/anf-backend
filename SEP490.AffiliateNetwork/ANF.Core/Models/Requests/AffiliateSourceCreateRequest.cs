using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    /// <summary>
    /// Publisher's affiliate source creating model
    /// </summary>
    public class AffiliateSourceCreateRequest
    {
        [Required(ErrorMessage = "Provider must be specified!", AllowEmptyStrings = false)]
        public string? Provider { get; set; }

        [Required(ErrorMessage = "Source's URL is required!", AllowEmptyStrings = false)]
        public string SourceUrl { get; set; } = null!;

        public string? Type { get; set; }
    }
}
