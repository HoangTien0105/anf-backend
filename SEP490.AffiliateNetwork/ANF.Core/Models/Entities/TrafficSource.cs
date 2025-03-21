using ANF.Core.Commons;
using ANF.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Affiliate source of a publisher
    /// </summary>
    public class TrafficSource : IEntity
    {
        [Column("pubs_no")]
        public long Id { get; set; }

        [Column("publisher_id")]
        public long PublisherId { get; set; }

        /// <summary>
        /// The name of the source's provider, like Facebook, Instagram, Tiktok, etc.
        /// </summary>
        [Column("provider")]
        public string? Provider { get; set; }

        [Column("soruce_url")]
        public string SourceUrl { get; set; } = null!;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Affiliate source status
        /// </summary>
        [Column("status")]
        public TrackingSourceStatus Status { get; set; }

        public User Publisher { get; set; } = null!;
    }
}
