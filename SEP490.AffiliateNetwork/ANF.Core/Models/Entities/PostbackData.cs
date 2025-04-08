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
        public string ClickId { get; set; } = null!;

        [Column("offer_id")]
        public long OfferId { get; set; }

        /// <summary>
        /// Transaction's id from advertiser
        /// </summary>
        [Column("transaction_id")]
        public string? TransactionId { get; set; }

        /// <summary>
        /// Postback created date
        /// </summary>
        [Column("date")]
        public DateTime? Date { get; set; }

        [Column("publisher_code")]
        public string? PublisherCode { get; set; }

        [Column("amount")]
        public double? Amount { get; set; }

        [Column("status")]
        public PostbackStatus? Status { get; set; }

        public Offer Offer { get; set; } = null!;
    }
}
