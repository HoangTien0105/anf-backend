namespace ANF.Core.Commons
{
    public class PaymentRedirectedPage
    {
        /// <summary>
        /// URL to redirect to after a successful payment
        /// </summary>
        public const string PaymentSuccessfulPage = "http://localhost:3000/advertiser/profile";

        /// <summary>
        /// URL to redirect to after cancel payment
        /// </summary>
        public const string PaymentCanceledPage = "http://localhost:3000/advertiser/profile";
    }
}
