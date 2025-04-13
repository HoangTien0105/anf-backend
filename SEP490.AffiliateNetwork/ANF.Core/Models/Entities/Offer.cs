using ANF.Core.Commons;
using ANF.Core.Enums;
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

        [Column("offer_description")]
        public string Description { get; set; } = null!;

        /// <summary>
        /// List of detailed instruction for publishers to get commission or money from offer
        /// </summary>
        [Column("step_info")]
        public string StepInfo { get; set; } = null!;

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Money gained by publisher for a click, order, etc. Based on the pricing model and advertiser rule
        /// </summary>
        [Column("bid", TypeName = "decimal(12)")]
        public decimal Bid { get; set; }

        /// <summary>
        /// The amount of money to run an offer
        /// </summary>
        [Column("budget", TypeName = "decimal(12)")]
        public decimal Budget { get; set; }

        [Column("commission_rate")]
        public double? CommissionRate { get; set; }

        [Column("order_return_time")]
        public string? OrderReturnTime { get; set; }

        [Column("img_url")]
        public string? ImageUrl { get; set; }

        [Column("status")]
        public OfferStatus? Status { get; set; }

        [Column("rejected_reason")]
        public string? RejectedReason { get; set; }

        [Column("concurrency_stamp")]
        [Timestamp]
        public byte[] ConcurrencyStamp { get; set; } = null!;

        public ICollection<PublisherOffer> PublisherOffers { get; set; } = new List<PublisherOffer>();

        public ICollection<PostbackData> PostbackData { get; set; } = new List<PostbackData>();

        public ICollection<TrackingEvent> TrackingEvents { get; set; } = new List<TrackingEvent>();

        public Campaign Campaign { get; set; } = null!;
    }
}
