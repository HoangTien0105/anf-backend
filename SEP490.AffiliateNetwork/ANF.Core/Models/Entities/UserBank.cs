using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    public class UserBank : IEntity
    {
        [Column("ub_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("user_code")]
        public Guid? UserCode { get; set; }

        [Column("banking_no")]
        public int BankingNo { get; set; }

        [Column("banking_provider")]
        public string BankingProvider { get; set; } = null!;

        [Column("added_date")]
        public DateTime AddedDate { get; set; }

        public User? User { get; set; }
    }
}
