namespace ANF.Core.Models.Responses
{
    public class LoginResponse
    {
        public long Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string AccessToken { get; set; } = null!;
    }
}
