namespace ANF.Core.Models.Responses
{
    /// <summary>
    /// Detailed information of the user
    /// </summary>
    public class DetailedUserResponse
    {
        public long Id { get; set; }

        public string UserCode { get; init; } = null!;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? CitizenId { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Email { get; set; } = null!;

        public string Role { get; set; } = null!;

        public decimal? Balance { get; set; }
        public decimal? CurrentBalance { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<UserBankResponse> BankResponses { get; set; } = [];

        public AdvertiserProfileInfoResponse? AdvertiserProfile { get; set; }

        public PublisherProfileInfoResponse? PublisherProfile { get; set; }

        public string? AccessToken { get; set; }
    }
}
