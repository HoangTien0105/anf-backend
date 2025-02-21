using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    /// <summary>
    /// Model to create advertiser profile
    /// </summary>
    public class AdvertiserProfileRequest
    {
        public string? CompanyName { get; set; }

        public string? Industry { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Banking number is required.", AllowEmptyStrings = false)]
        public string BankingNo { get; set; } = null!;

        [Required(ErrorMessage = "Banking provider is required.", AllowEmptyStrings = false)]
        public string? BankingProvider { get; set; }

        public string? Bio { get; set; }

        [Required(ErrorMessage = "Advertiser's id is required.")]
        public long AdvertiserId { get; set; }
    }
}
