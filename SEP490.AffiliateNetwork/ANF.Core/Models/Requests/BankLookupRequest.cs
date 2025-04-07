namespace ANF.Core.Models.Requests
{
    public class BankLookupRequest
    {
        /// <summary>
        /// Bank's code
        /// </summary>
        public string BankCode { get; init; } = null!;

        /// <summary>
        /// Bank's account number
        /// </summary>
        public string AccountNo { get; set; } = null!;
    }
}
