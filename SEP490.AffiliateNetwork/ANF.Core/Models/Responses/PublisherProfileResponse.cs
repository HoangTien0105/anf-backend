using ANF.Core.Enums;

namespace ANF.Core.Models.Responses
{
    public class PublisherProfileResponse
    {
        public long Id { get; set; }

        public string UserCode { get; set; } = null!;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? CitizenId { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Status { get; set; }

        public string? Specialization { get; set; }

        public string? ImageUrl { get; set; }

        public string? Bio { get; set; }
    }
}
