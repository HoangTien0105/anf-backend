using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    public class BatchPayment : IEntity
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("transaction_id")]
        public long TransactionId { get; set; }

        [Column("from_account")]
        public string FromAccount { get; set; } = null!;

        [Column("amount", TypeName = "decimal(12)")]
        public decimal Amount { get; set; }

        [Column("beneficiary_name")]
        public string BeneficiaryName { get; set; } = null!;

        [Column("beneficiary_account")]
        public string BeneficiaryAccount { get; set; } = null!;

        [Column("reason")]
        public string Reason { get; set; } = null!;

        [Column("beneficiary_bank_code")]
        public string BeneficiaryBankCode { get; set; } = null!;

        [Column("beneficiary_bank_name")]
        public string BeneficiaryBankName { get; set; } = null!;

        [Column("date")]
        public DateTime? Date { get; set; }
    }
}
