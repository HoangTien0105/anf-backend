using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Information about a subscription
    /// </summary>
    public class Subscription : IEntity
    {
        /// <summary>
        /// Id of the subscription, not auto incremment
        /// </summary>
        [Column("sub_id")]
        public long Id { get; set; }

        [Column("sub_name")]
        public string Name { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }

        [Column("sub_price")]
        public double Price { get; set; }

        [Column("duration")]
        public string? Duration { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}
