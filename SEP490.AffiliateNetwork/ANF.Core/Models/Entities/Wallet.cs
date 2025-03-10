using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    public class Wallet : IEntity
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("balance")]
        public double Balance { get; set; } = 0;

        [Column("is_active")]
        public bool IsActive { get; set; } = false;

        [Column("user_id")]
        public long UserId { get; set; }

        public User User { get; set; } = null!;

        public ICollection<WalletHistory> WalletHistories { get; set; } = new List<WalletHistory>();

        public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new List<PaymentTransaction>();
    }
}
