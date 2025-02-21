using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class LoginRequest
    {
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public required string Email { get; init; }
        
        public required string Password { get; init; }
    }
}
