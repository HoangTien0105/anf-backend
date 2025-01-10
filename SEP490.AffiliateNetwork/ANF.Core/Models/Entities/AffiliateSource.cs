using ANF.Core.Commons;

namespace ANF.Core.Models.Entities
{
    public class AffiliateSource : IEntity
    {
        public int Id { get; set; }

        public Guid PublisherId { get; set; }

        /// <summary>
        /// The name of the source's provider, like Facebook, Instagram, Tiktok, etc.
        /// </summary>
        public string? Provider { get; set; }

        public string SourceUrl { get; set; } = null!;

        public byte[]? CreatedAt { get; set; }

        public byte[]? UpdatedAt { get; set; }
    }
}
