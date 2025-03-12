using ANF.Core.Commons;
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

        //TODO: Review the field again. Use publisher_id or publisher_code?
        [Column("publisher_id")]
        public long PublisherId { get; set; }

        [Column("amount")]
        public double? Amount { get; set; }

        //TODO: Define status for postback
        [Column("status")]
        public int Status { get; set; }

        public Offer Offer { get; set; } = null!;
    }
}
