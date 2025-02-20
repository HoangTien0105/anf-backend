using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class PublisherProfileRequest
    {
        public string? Specialization { get; set; }

        public string? ImageUrl { get; set; }

        public string BankingNo { get; set; } = null!;

        public string? BankingProvider { get; set; }

        public string? Bio { get; set; }

        [Required]
        public long PublisherId { get; set; }
    }
}
