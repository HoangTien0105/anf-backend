using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class PublisherProfileRequest
    {
        public string? Specialization { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Banking number is required.", AllowEmptyStrings = false)]
        public string BankingNo { get; set; } = null!;

        [Required(ErrorMessage = "Banking provider is required.", AllowEmptyStrings = false)]
        public string? BankingProvider { get; set; }

        public string? Bio { get; set; }

        [Required(ErrorMessage = "Publisher's id is required.")]
        public long PublisherId { get; set; }
    }
}
