namespace ANF.Core.Models.Responses
{
    /// <summary>
    /// Advertiser response model
    /// </summary>
    public class AdvertiserProfileResponse
    {
        public long Id { get; set; }

        public Guid AdvertiserCode { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? CitizenId { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Email { get; set; } = null!;

        public string? CompanyName { get; set; }

        public string? Industry { get; set; }

        public string? ImageUrl { get; set; }

        public string? Bio { get; set; }

        public ICollection<UserBankResponse> UserBanks { get; set; } = new List<UserBankResponse>();
    }
}
