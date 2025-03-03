using Amazon.S3;
using Amazon.S3.Model;
using ANF.Core.Commons;
using ANF.Core.Services;
using Microsoft.AspNetCore.Http;
using R2.NET.Factories;

namespace ANF.Service
{
    public class ImageService() : IImageService
    {
        //WARNING: Cannot run the method, fix fix fix
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            // Generate unique file name
            //var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            //var client = _clientFactory.GetClient("r2-client", default);
            
            // Upload to Cloudflare R2
            //using var stream = file.OpenReadStream();
            //var response = await client.UploadBlobAsync((FileStream)stream, fileName, default);

            // Construct the URL
            //var imageUrl = $"{CloudflareR2Constants.BaseUrl}/{CloudflareR2Constants.BucketName}/{fileName}";
            //return imageUrl;
            throw new NotImplementedException();
        }
    }
}
