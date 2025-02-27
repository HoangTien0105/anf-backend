namespace ANF.Core.Models.Responses
{
    public class AffiliateSourceResponse
    {
        public long Id { get; set; }

        public string? Provider { get; set; }

        public string SourceUrl { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string? Type { get; set; }
    }
}
