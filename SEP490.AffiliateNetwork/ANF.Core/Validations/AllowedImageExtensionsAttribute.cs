using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ANF.Core.Validations
{
    public class AllowedImageExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public AllowedImageExtensionsAttribute(string errorMessage = "Only image files (jpg, png, gif, webp, svg, avif) are allowed.")
        {
            _allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg", ".avif" };
            ErrorMessage = errorMessage;
        }

        public AllowedImageExtensionsAttribute(string[] extensions, string errorMessage = "Unsupported file format.")
        {
            _allowedExtensions = extensions.Select(e => e.StartsWith(".") ? e.ToLowerInvariant() : $".{e.ToLowerInvariant()}").ToArray();
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!_allowedExtensions.Contains(extension))
                {
                    return new ValidationResult(ErrorMessage);
                }

                // Optional: You could also verify the content type if needed
                if (!IsValidContentType(file.ContentType))
                {
                    return new ValidationResult($"Invalid content type: {file.ContentType}");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid file type. Expected IFormFile.");
        }

        private bool IsValidContentType(string contentType)
        {
            var validContentTypes = new[]
            {
                "image/jpeg",
                "image/jpg",
                "image/png",
                "image/gif",
                "image/webp",
                "image/svg+xml",
                "image/avif"
            };

            return validContentTypes.Contains(contentType.ToLowerInvariant());
        }
    }
}
