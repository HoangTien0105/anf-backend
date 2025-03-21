using ANF.Core.Commons;
using ANF.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Data sent back to the system by advertiser
    /// </summary>
    public class PostbackData : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("pbd_no")]
        public long Id { get; set; }

        [Column("click_id")]
        public Guid ClickId { get; set; }

        [Column("offer_id")]
        public long OfferId { get; set; }

        [Column("publisher_code")]
        public string PublisherCode { get; set; } = null!;

        [Column("amount")]
        public double? Amount { get; set; }

        [Column("status")]
        public PostbackStatus? Status { get; set; }

        public Offer Offer { get; set; } = null!;
    }
}
