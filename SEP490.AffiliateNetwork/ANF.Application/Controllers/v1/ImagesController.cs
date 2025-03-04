using ANF.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ANF.Application.Controllers.v1
{
    /// <summary>
    /// Test cloudinary service
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ICloudinaryService _imageService;

        public ImagesController(ICloudinaryService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                var imageUrl = await _imageService.UploadImageAsync(file);
                return Ok(new { Url = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading image: {ex.Message}");
            }
        }
    }
}
