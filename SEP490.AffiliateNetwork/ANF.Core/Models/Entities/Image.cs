using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Table to store images of campaigns and offers
    /// </summary>
    public class Image : IEntity
    {
        [Column("img_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("offer_id")]
        public long? OfferId { get; set; }

        [Column("campaign_id")]
        public long? CampaignId { get; set; }

        [Column("thumbnail")]
        public string? Thumbnail { get; set; }

        [Column("img_url")]
        public string? ImageUrl { get; set; }

        public Campaign? Campaign { get; set; }

        public Offer? Offer { get; set; }
    }
}
