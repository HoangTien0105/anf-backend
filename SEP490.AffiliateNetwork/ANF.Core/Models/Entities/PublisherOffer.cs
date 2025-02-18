using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    public class PublisherOffer : IEntity
    {
        [Column("offer_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("offer_id")]
        public long OfferId { get; set; }

        [Column("offer_id")]
        public long PublisherId { get; set; }

        [Column("offer_id")]
        public DateTime JoiningDate { get; set; }

        public User Publisher { get; set; } = null!;

        public Offer Offer { get; set; } = null!;
    }
}
