using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    // TODO: Add relationship for this entity
    public class WalletHistory
    {
        [Column("wh_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("amount")]
        public double Amount { get; set; }

        [Column("type")]
        /// <summary>
        /// Type of transaction - DEPOSIT, WITHDRAW, CAMPAIGN REGISTRATION FEE
        /// </summary>
        public string? Type { get; set; }

        [Column("type_id")]
        /// <summary>
        /// Unique value of transaction type: camp_id, transaction_id, camp_fee
        /// </summary>
        public string? TypeId { get; set; }
    }
}
