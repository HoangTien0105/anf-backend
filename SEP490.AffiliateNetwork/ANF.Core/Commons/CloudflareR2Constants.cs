namespace ANF.Core.Commons
{
    public class CloudflareR2Constants
    {
        /// <summary>
        /// Cloudflare account's id
        /// </summary>
        public const string AccountId = "your-account-id";

        /// <summary>
        /// Bucket's name
        /// </summary>
        public const string BucketName = "sep490-affiliate-network";

        /// <summary>
        /// Access key Id
        /// </summary>
        public const string AccessKeyId = "your-access-key-id";

        /// <summary>
        /// Secret access key
        /// </summary>
        public const string SecretAccessKey = "your-secret-access-key";

        /// <summary>
        /// Default endpoint of Cloudflare
        /// </summary>
        public const string BaseUrl = $"https://{AccountId}.r2.cloudflarestorage.com";
        
        /// <summary>
        /// Development endpoint to view objects in the bucket
        /// </summary>
        //public const string DevUrl = $"https://pub-e3f858629e99459c93d4efc1687f66bf.r2.dev";
    }
}
