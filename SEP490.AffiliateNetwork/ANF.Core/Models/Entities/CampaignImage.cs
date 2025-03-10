using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    public class CampaignImage : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("img_no")]
        public long Id { get; set; }

        [Column("camp_id")]
        public long? CampaignId { get; set; }

        [Column("img_url")]
        public string? ImageUrl { get; set; }
        
        [Column("added_at")]
        public DateTime AddedAt { get; set; }

        public Campaign? Campaign { get; set; }
    }
}
