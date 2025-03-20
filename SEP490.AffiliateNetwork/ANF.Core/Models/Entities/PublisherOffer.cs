using ANF.Core.Commons;
using ANF.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    public class PublisherOffer : IEntity
    {
        [Column("po_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("offer_id")]
        public long OfferId { get; set; }

        [Column("publisher_code")]
        public Guid PublisherCode { get; set; }

        [Column("joining_date")]
        public DateTime JoiningDate { get; set; }

        /// <summary>
        /// Approved date (Admin)
        /// </summary>
        [Column("approved_date")]
        public DateTime? ApprovedDate { get; set; }

        [Column("reject_reason")]
        public string? RejectReason { get; set; }
        
        [Column("status")]
        public PublisherOfferStatus Status { get; set; }

        public User Publisher { get; set; } = null!;

        public Offer Offer { get; set; } = null!;
    }
}
