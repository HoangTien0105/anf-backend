using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    public class Offer : IEntity
    {
        [Column("offer_id")]
        public long Id { get; set; }

        [Column("camp_id")]
        public long CampaignId { get; set; }

        [Column("pricing_model")]
        public string? PricingModel { get; set; }

        [Column("offer_note")]
        public string? Note { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Timestamp]
        public byte[] ConcurrencyStamp { get; set; } = null!;

        public Campaign Campaign { get; set; } = null!;
    }
}
