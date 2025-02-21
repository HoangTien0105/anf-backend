using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    /// <summary>
    /// Request model for creating a new account
    /// </summary>
    public class AccountCreateRequest
    {
        [Required(ErrorMessage = "First name is required.", AllowEmptyStrings = false)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.", AllowEmptyStrings = false)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required.", AllowEmptyStrings = false)]
        [RegularExpression(@"^\d{10,12}$",
            ErrorMessage = "Phone number must contain only numbers and be between 10 and 12 digits.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Citizen number is required.", AllowEmptyStrings = false)]
        [RegularExpression(@"^\d{12}$",
            ErrorMessage = "Citizen id must be exactly 12 digits.")]
        public string CitizenId { get; set; } = null!;

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required.", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.", AllowEmptyStrings = false)]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be in 8-16 characters.")]
        [RegularExpression(@"^(?=.*[!@#$%^&*(),.?""':;{}|<>]).*$",
            ErrorMessage = "Password must contain at least one special character (e.g., !@#$%^&*).")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Please re-enter the password again.", AllowEmptyStrings = false)]
        public string PasswordConfirmed { get; set; } = null!;

        [Required(ErrorMessage = "User's role is required.", AllowEmptyStrings = false)]
        public string Role { get; set; } = null!;
    }
}
