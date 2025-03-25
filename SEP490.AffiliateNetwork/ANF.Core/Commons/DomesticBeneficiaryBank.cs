namespace ANF.Core.Commons
{
    /// <summary>
    /// Ngân hàng dùng cho Mẫu Thanh toán - Lô trong và ngoài Techcombank
    /// </summary>
    public class DomesticBeneficiaryBank
    {
        public string BankCode { get; set; } = null!;
        
        public string BankName { get; set; } = null!;
        
        public string Location { get; set; } = null!;
        
        public string BranchName { get; set; } = null!;
    }
}
