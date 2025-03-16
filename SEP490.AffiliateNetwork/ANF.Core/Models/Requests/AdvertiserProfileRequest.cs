using ANF.Core.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    /// <summary>
    /// Model to create advertiser profile
    /// </summary>
    public class AdvertiserProfileRequest
    {
        [BindProperty(Name = "companyName")]
        public string? CompanyName { get; set; }

        [BindProperty(Name = "industry")]
        public string? Industry { get; set; }

        [BindProperty(Name = "image")]
        public IFormFile? Image { get; set; }

        [BindProperty(Name = "bio")]
        [AllowedImageExtensions]
        public string? Bio { get; set; }

        [Required(ErrorMessage = "Advertiser's id is required.")]
        [BindProperty(Name = "advertiserId")]
        public long AdvertiserId { get; set; }
    }
}
