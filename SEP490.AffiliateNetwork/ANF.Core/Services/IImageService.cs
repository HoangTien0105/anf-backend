using Microsoft.AspNetCore.Http;

namespace ANF.Core.Services
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
