using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    public class Wallet : IEntity
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("balance", TypeName = "decimal(10,2)")]
        public decimal Balance { get; set; } = 0;

        [Column("is_active")]
        public bool IsActive { get; set; } = false;

        [Column("user_code")]
        public string UserCode { get; set; } = null!;

        public User User { get; set; } = null!;
            
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
