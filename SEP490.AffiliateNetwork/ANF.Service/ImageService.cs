using Amazon.S3;
using Amazon.S3.Model;
using ANF.Core.Commons;
using ANF.Core.Services;
using Microsoft.AspNetCore.Http;

namespace ANF.Service
{
    public class ImageService(IAmazonS3 amazonS3) : IImageService
    {
        private readonly IAmazonS3 _amazonS3 = amazonS3;
        private readonly string _bucketName = "sep490-affiliate-network";

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            // Generate unique file name
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            // Upload to Cloudflare R2
            using var stream = file.OpenReadStream();
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                InputStream = stream,
                ContentType = file.ContentType,
                DisablePayloadSigning = true,
            };

            // Chạy line này xong sẽ error: The request signature we calculated does not match the signature you provided. Check your secret access key and signing method.
            await _amazonS3.PutObjectAsync(putRequest);

            // Construct the URL (đã enable endpoint của dev)
            var imageUrl = $"{CloudflareR2Constants.DevUrl}/{fileName}";
            return imageUrl;
        }
    }
}
