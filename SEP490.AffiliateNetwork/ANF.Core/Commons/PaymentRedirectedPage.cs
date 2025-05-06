namespace ANF.Core.Commons
{
    public class PaymentRedirectedPage
    {
        /// <summary>
        /// URL to redirect to after a successful payment
        /// </summary>
        public const string PaymentSuccessfulPage = "https://dev.l3on.id.vn/payments";

        /// <summary>
        /// URL to redirect to after cancel payment
        /// </summary>
        public const string PaymentCanceledPage = "https://dev.l3on.id.vn/payments/failed";

        /// <summary>
        /// URL to redirect to campaign
        /// </summary>
        public const string CampaignRedirectPageForAdvertiser = "https://dev.l3on.id.vn/advertiser/campaigns/{0}";

        /// <summary>
        /// URL to redirect to offer after publisher request to join an offer for advertiser
        /// </summary>
        public const string OfferRedirectPageForAdvertiser = "https://dev.l3on.id.vn/advertiser/campaigns/{0}/offers/{1}";

        /// <summary>
        /// URL to redirect to offer after advertiser accepted request to join an offer for publisher
        /// </summary>
        public const string OfferRedirectPageForPublisher = "https://dev.l3on.id.vn/publisher/campaigns/{0}}";

        public const string SignInRedirectPage = "https://dev.l3on.id.vn/auth/sign-in";
    }
}
