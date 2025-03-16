using ANF.Core.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    /// <summary>
    /// Updated request for publisher
    /// </summary>
    public class PublisherProfileUpdatedRequest
    {
        [Required(ErrorMessage = "Phone number is required!", AllowEmptyStrings = false)]
        [BindProperty(Name = "phoneNumber")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Citizen number is required.", AllowEmptyStrings = false)]
        [RegularExpression(@"^\d{12}$",
            ErrorMessage = "Citizen id must be exactly 12 digits.")]
        [BindProperty(Name = "citizenId")]
        public string? CitizenId { get; set; }

        [BindProperty(Name = "address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [BindProperty(Name = "dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Date of birth is required.", AllowEmptyStrings = false)]
        [BindProperty(Name = "specialization")]
        public string? Specialization { get; set; }

        [BindProperty(Name = "image")]
        [AllowedImageExtensions]
        public IFormFile? Image { get; set; }

        [BindProperty(Name = "bio")]
        public string? Bio { get; set; }
    }
}
