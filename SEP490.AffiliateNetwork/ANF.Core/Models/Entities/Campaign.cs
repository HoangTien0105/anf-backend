using ANF.Core.Commons;
using ANF.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Information of a campaign
    /// </summary>
    public class Campaign : IEntity
    {
        [Column("camp_id")]
        public long Id { get; set; }

        [Column("advertiser_id")]
        public long AdvertiserId { get; set; }

        [Column("camp_name")]
        public string Name { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("budget")]
        public double Budget { get; set; }

        [Column("balance")]
        public double Balance { get; set; }

        [Column("product_url")]
        public string ProductUrl { get; set; } = null!;

        [Column("tracking_params", TypeName = "text")]
        public string? TrackingParams { get; set; }

        [Column("cate_id")]
        public long? CategoryId { get; set; }

        [Column("camp_status")]
        public CampaignStatus Status { get; set; }

        [Column("concurrency_stamp")]
        [Timestamp]
        public byte[] ConcurrencyStamp { get; set; } = null!;

        public User Advertiser { get; set; } = null!;

        public Category? Category { get; set; }

        public ICollection<Offer>? Offers { get; set; }
        
        public ICollection<Image>? Images { get; set; }
    }
}
