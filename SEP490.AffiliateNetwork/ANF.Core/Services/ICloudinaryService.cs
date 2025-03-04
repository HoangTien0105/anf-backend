using Microsoft.AspNetCore.Http;

namespace ANF.Core.Services
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
