namespace ANF.Core.Models.Responses
{
    public class PublisherOfferResponse
    {
        public long Id { get; init; }

        public string PublisherCode { get; init; } = null!;

        public string? FirstName { get; init; }

        public string? LastName { get; init; }

        public string? PhoneNumber { get; init; }

        public string? CitizenId { get; init; }

        public string? Address { get; init; }

        public DateTime? DateOfBirth { get; init; }

        public string Email { get; init; } = null!;
    }
}
