using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class SubscriptionPurchaseRequest
    {
        /// <summary>
        /// Subscription id
        /// </summary>
        [Required(ErrorMessage = "Subscription id is required!")]
        public long Id { get; set; }

        /// <summary>
        /// Subscription price
        /// </summary>
        [Required(ErrorMessage = "Subscription price is required!")]
        public decimal Price { get; set; }
    }
}
