namespace ANF.Core.Commons
{
    public class CloudflareR2Constants
    {
        /// <summary>
        /// Cloudflare account's id
        /// </summary>
        public const string AccountId = "1f4d69b646e18427d2669a1c9fa986c2";

        /// <summary>
        /// Access key
        /// </summary>
        public const string AccessKey = "3e9c1d4b7841978082300c7abe0d6c63";

        /// <summary>
        /// Secret key
        /// </summary>
        public const string SecretKey = "a56e670f95cb883bd9de7a8ea2c11843bca6455b6466406661501939b1effc35";

        /// <summary>
        /// Default endpoint of Cloudflare
        /// </summary>
        public const string BaseUrl = $"https://{AccountId}.r2.cloudflarestorage.com";

        /// <summary>
        /// Default endpoint of the bucket
        /// </summary>
        public const string BucketUrl = $"https://{AccountId}.r2.cloudflarestorage.com/sep490-affiliate-network";

        /// <summary>
        /// Development endpoint to view objects in the bucket
        /// </summary>
        public const string DevUrl = $"https://pub-e3f858629e99459c93d4efc1687f66bf.r2.dev";
    }
}
