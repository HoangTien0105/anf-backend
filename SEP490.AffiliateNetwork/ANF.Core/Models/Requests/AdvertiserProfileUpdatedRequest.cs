using ANF.Core.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    /// <summary>
    /// Request model for updating advertiser's profile
    /// </summary>
    public class AdvertiserProfileUpdatedRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone number is required!")]
        [RegularExpression(@"^\d{10,12}$",
            ErrorMessage = "Phone number must contain only numbers and be between 10 and 12 digits.")]
        [BindProperty(Name = "phoneNumber")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Citizen number is required!", AllowEmptyStrings = false)]
        [RegularExpression(@"^\d{12}$",
            ErrorMessage = "Citizen id must be exactly 12 digits.")]
        [BindProperty(Name = "citizenId")]
        public string? CitizenId { get; set; }

        [BindProperty(Name = "address")]
        public string? Address { get; set; }

        [BindProperty(Name = "dateOfBirth")]
        [Required(ErrorMessage = "Date of birth is required!")]
        public DateTime? DateOfBirth { get; set; }

        [BindProperty(Name = "companyName")]
        [Required(ErrorMessage = "Company name is required!", AllowEmptyStrings = false)]
        public string? CompanyName { get; set; }

        [BindProperty(Name = "industry")]
        [Required(ErrorMessage = "Industry is required!", AllowEmptyStrings = false)]
        public string? Industry { get; set; }

        [BindProperty(Name = "image")]
        [AllowedImageExtensions]
        public IFormFile? Image { get; set; }

        [BindProperty(Name = "bio")]
        public string? Bio { get; set; }
    }
}
