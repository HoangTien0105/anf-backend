using ANF.Core.Commons;
using ANF.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    public class PaymentTransaction : IEntity
    {
        [Column("trans_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("user_code")]
        public Guid UserCode { get; set; }

        [Column("wallet_id")]
        public long WalletId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("amount")]
        public double Amount { get; set; }

        /// <summary>
        /// Payment status: Success, Failed, Canceled
        /// </summary>
        [Column("payment_status")]
        public PaymentStatus Status { get; set; }

        public ICollection<WalletHistory>? WalletHistories { get; set; }

        public Wallet? Wallet { get; set; }

        public User? User { get; set; }
    }
}
