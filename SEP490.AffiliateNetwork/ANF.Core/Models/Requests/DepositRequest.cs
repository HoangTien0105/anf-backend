using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    /// <summary>
    /// Model for deposit money into wallet
    /// </summary>
    public class DepositRequest
    {
        //[Required(ErrorMessage = "User's code is required!", AllowEmptyStrings = false)]
        //public string UserCode { get; set; } = null!;

        //[Required(ErrorMessage = "Wallet's id is required!")]
        //public long WalletId { get; set; }

        [Range(2001, double.MaxValue, ErrorMessage = "The amount to be deposited into the wallet must be 2.000 VND or more")]
        public decimal Amount { get; set; }
    }
}
