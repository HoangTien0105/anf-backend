using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    public class WalletHistory : IEntity
    {
        [Column("wh_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Current wallet balance at the time when transaction occurs
        /// </summary>
        [Column("transaction_id")]
        public long? TransactionId { get; set; }

        /// <summary>
        /// Current wallet balance at the time when transaction occurs
        /// </summary>
        [Column("current_balance")]
        public double? CurrentBalance { get; set; }

        /// <summary>
        /// Money changes: if current_balance > balance then money is decreased otherwise.
        /// </summary>
        [Column("balance_type")]
        public bool BalanceType { get; set; }

        public Transaction? Transaction { get; set; }
    }
}
