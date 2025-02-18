using ANF.Core.Commons;
using ANF.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Affiliate source of a publisher
    /// </summary>
    public class PublisherSource : IEntity
    {
        [Column("")]
        public long Id { get; set; }

        [Column("")]
        public long PublisherId { get; set; }

        /// <summary>
        /// The name of the source's provider, like Facebook, Instagram, Tiktok, etc.
        /// </summary>
        [Column("")]
        public string? Provider { get; set; }

        [Column("")]
        public string SourceUrl { get; set; } = null!;

        [Column("")]
        public DateTime CreatedAt { get; set; }

        [Column("")]
        public string? Type { get; set; }

        /// <summary>
        /// Affiliate source status
        /// </summary>
        [Column("")]
        public AffSourceStatus Status { get; set; }

        public User Publisher { get; set; } = null!;
    }
}
