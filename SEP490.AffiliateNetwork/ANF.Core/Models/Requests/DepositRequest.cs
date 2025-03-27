using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    /// <summary>
    /// Model for deposit money into wallet
    /// </summary>
    public class DepositRequest
    {
        [Range(2001, double.MaxValue, ErrorMessage = "The amount to be deposited into the wallet must be 2.000 VND or more")]
        public decimal Amount { get; set; }
    }
}
