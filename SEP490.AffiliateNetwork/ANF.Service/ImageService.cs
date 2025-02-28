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
        
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            // Generate unique file name
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            // Upload to Cloudflare R2
            using var stream = file.OpenReadStream();
            var putRequest = new PutObjectRequest
            {
                BucketName = CloudflareR2Constants.BucketName,
                Key = fileName,
                InputStream = stream,
                ContentType = file.ContentType,
                //DisablePayloadSigning = true,
            };

            await _amazonS3.PutObjectAsync(putRequest);

            // Construct the URL (đã enable endpoint của dev)
            var imageUrl = $"{CloudflareR2Constants.BaseUrl}/{CloudflareR2Constants.BucketName}/{fileName}";
            return imageUrl;
        }
    }
}
