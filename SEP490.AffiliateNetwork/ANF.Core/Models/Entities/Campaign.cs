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

        [Column("advertiser_code")]
        public string AdvertiserCode { get; set; } = null!;

        [Column("camp_name")]
        public string Name { get; set; } = null!;

        [Column("description", TypeName = "ntext")]
        public string Description { get; set; } = null!;

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Budget of a campaign
        /// </summary>
        [Column("budget", TypeName = "decimal(12)")]
        public decimal? Budget { get; set; }

        /// <summary>
        /// Balance = Sum of offers' budget
        /// </summary>
        [Column("balance", TypeName = "decimal(12)")]
        public decimal? Balance { get; set; }

        [Column("product_url")]
        public string ProductUrl { get; set; } = null!;

        [Column("tracking_params", TypeName = "text")]
        public string? TrackingParams { get; set; }

        [Column("reject_reason")]
        public string? RejectReason { get; set; }

        [Column("cate_id")]
        public long? CategoryId { get; set; }

        [Column("camp_status")]
        public CampaignStatus Status { get; set; }

        [Column("concurrency_stamp")]
        [Timestamp]
        public byte[] ConcurrencyStamp { get; set; } = null!;

        public User Advertiser { get; set; } = null!;

        public Category? Category { get; set; }

        public ICollection<CampaignImage>? Images { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }

        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}
