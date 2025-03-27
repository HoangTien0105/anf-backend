namespace ANF.Core.Models.Responses
{
    public class ExportedBatchDataResponse
    {
        public string TransactionId { get; set; } = null!;
        
        public string FromAccount { get; set; } = null!;
        
        public decimal Amount { get; set; }
        
        public string BeneficiaryName { get; set; } = null!;
        
        public string BeneficiaryAccount { get; set; } = null!;
        
        public string Reason { get; set; } = null!;
        
        public string BeneficiaryBankCode { get; set; } = null!;
        
        public string BeneficiaryBankName { get; set; } = null!;
    }
}
