using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    // TODO: Add relationship for this entity
    public class Wallet : IEntity
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("balance")]
        public double Balance { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = false;
    }
}
