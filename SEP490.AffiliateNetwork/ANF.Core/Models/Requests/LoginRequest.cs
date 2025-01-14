using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Requests
{
    public class LoginRequest
    {
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
