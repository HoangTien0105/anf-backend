using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    public class PublisherProfile : IEntity
    {
        [Column("pub_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("specialization")]
        public string? Specialization { get; set; }

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Column("banking_no")]
        public string BankingNo { get; set; } = null!;

        [Column("banking_provider")]
        public string? BankingProvider { get; set; }

        [Column("bio")]
        public string? Bio { get; set; }

        [Column("publisher_id")]
        public long PublisherId { get; set; }

        public User Publisher { get; set; } = null!;
    }
}
