using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    public class AdvertiserProfile : IEntity
    {
        [Column("adv_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("company_name")]
        public string? CompanyName { get; set; }

        [Column("industry")]
        public string? Industry { get; set; }

        [Column("image_url")]
        public string? ImageUrl { get; set; }

        [Column("bio")]
        public string? Bio { get; set; }

        [Column("advertiser_id")]
        public long AdvertiserId { get; set; }

        public User Advertiser { get; set; } = null!;
    }
}
