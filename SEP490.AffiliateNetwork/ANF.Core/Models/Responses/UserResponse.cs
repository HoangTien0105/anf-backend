namespace ANF.Core.Models.Responses
{
    /// <summary>
    /// Response model for user (admin role)
    /// </summary>
    public class UserResponse
    {
        public long Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? CitizenId { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Email { get; set; } = null!;

        public bool? EmailConfirmed { get; set; }

        public string Status { get; set; } = null!;

        public string Role { get; set; } = null!;
    }
}
