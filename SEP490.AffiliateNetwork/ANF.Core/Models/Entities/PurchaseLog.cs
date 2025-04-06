using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Save the order's information of user before payment into platform 
    /// </summary>
    public class PurchaseLog : IEntity
    {
        [Column("id")]
        public long Id { get; set; }

        /// <summary>
        /// Mapping with click's id at tracking event table
        /// </summary>
        [Column("click_id")]
        public string? ClickId { get; set; }

        [Column("transaction_id")]
        public string? TransactionId { get; set; }

        /// <summary>
        /// The name of the item basing on the pricing model
        /// CPS: Item's name
        /// CPA: Form's name
        /// </summary>
        [Column("item_name")]
        public string? ItemName { get; set; }

        /// <summary>
        /// For CPS model
        /// </summary>
        [Column("quantity")]
        public int? Quantity { get; set; }

        [Column("amount", TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }
    }
}
