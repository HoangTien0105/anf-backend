namespace ANF.Core.Models.Responses
{
    public class PublisherInformationForAdvertiser
    {
        public string PublisherCode { get; set; } = null!;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? CitizenId { get; set; }

        public string? Address { get; set; }

        public string Email { get; set; } = null!;

        public string? Specialization { get; set; }

        public string? NoOfExperience { get; set; }

        public string? ImageUrl { get; set; }

        public string? Bio { get; set; }

        public ICollection<PublisherTrafficSource> TrafficSources { get; set; } = [];
    }

    public class PublisherTrafficSource
    {
        public string? Provider { get; set; }

        public string SourceUrl { get; set; } = null!;

        public string? Type { get; set; }
    }
}
