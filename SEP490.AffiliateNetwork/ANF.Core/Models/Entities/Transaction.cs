using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    // TODO: Add relationship for this entity
    public class Transaction : IEntity
    {
        [Column("trans_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("amount")]
        public double Amount { get; set; }

        [Column("status")]
        // TODO: Need to define more information - SUCCESS, FAIL, CANCEL
        public int Status { get; set; }
    }
}
