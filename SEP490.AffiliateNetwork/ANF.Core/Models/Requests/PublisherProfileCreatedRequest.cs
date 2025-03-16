using ANF.Core.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class PublisherProfileCreatedRequest
    {
        [Required(ErrorMessage = "Specialization is required!", AllowEmptyStrings = false)]
        [BindProperty(Name = "specialization")]
        public string Specialization { get; set; } = null!;

        [BindProperty(Name = "image")]
        [AllowedImageExtensions]
        public IFormFile? Image { get; set; }

        [BindProperty(Name = "bio")]
        public string? Bio { get; set; }
        
        [Required(ErrorMessage = "Publisher's id is required.")]
        [BindProperty(Name = "publisherId")]
        public long PublisherId { get; set; }
    }
}
