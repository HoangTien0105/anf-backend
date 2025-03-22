using System.Text.Json.Serialization;

namespace ANF.Core.Models.Responses
{
    public class PublisherOfferResponse
    {
        [JsonPropertyName("poNo")]
        public long Id { get; init; }

        public long PublisherId { get; init; }

        public string PublisherCode { get; init; } = null!;

        public long OfferId { get; init; }

        public string? FirstName { get; init; }

        public string? LastName { get; init; }

        public string? PhoneNumber { get; init; }

        public string Email { get; init; } = null!;

        public ICollection<PublisherOfferTrafficSource>? TrafficSources { get; set; }
    }

    public class PublisherOfferTrafficSource
    {
        public string? Provider { get; init; }

        public string SourceUrl { get; init; } = null!;

        public string? Type { get; init; }
    }
}
