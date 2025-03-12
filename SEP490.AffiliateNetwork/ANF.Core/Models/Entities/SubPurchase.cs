using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Subscription purchases
    /// </summary>
    public class SubPurchase : IEntity
    {
        [Column("subp_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("advertiser_code")]
        public Guid? AdvertiserCode { get; set; }

        [Column("sub_id")]
        public long? SubscriptionId { get; set; }

        [Column("purchased_at")]
        public DateTime PurchasedAt { get; set; }

        [Column("expired_at")]
        public DateTime? ExpiredAt { get; set; }

        [Column("current_price")]
        public double CurrentPrice { get; set; }

        public Subscription? Subscription { get; set; }

        public User? Advertiser { get; set; }
    }
}
