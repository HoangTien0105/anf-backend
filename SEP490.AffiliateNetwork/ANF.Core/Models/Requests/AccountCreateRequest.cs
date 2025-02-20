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
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Citizen number is required.", AllowEmptyStrings = false)]
        public string CitizenId { get; set; } = null!;

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required.", AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.", AllowEmptyStrings = false)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Please re-enter the password again.", AllowEmptyStrings = false)]
        public string PasswordConfirmed { get; set; } = null!;

        public string Role { get; set; } = null!;
    }
}
