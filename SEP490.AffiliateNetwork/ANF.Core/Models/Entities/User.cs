using ANF.Core.Commons;
using ANF.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    public class User : IEntity
    {
        /// <summary>
        /// User's id, not auto increment
        /// </summary>
        [Column("user_id")]
        public long Id { get; set; }

        /// <summary>
        /// User's code for identifying the user
        /// UNIQUE value
        /// </summary>
        [Column("user_code")]
        public Guid UserCode { get; set; }

        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [Column("citizen_id")]
        public string? CitizenId { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// UNIQUE value
        /// </summary>
        [Column("user_email")]
        public string Email { get; set; } = null!;

        [Column("user_password")]
        public string Password { get; set; } = null!;

        [Column("email_confirmed")]
        public bool? EmailConfirmed { get; set; }

        [Column("user_status")]
        public UserStatus Status { get; set; }

        [Column("user_role")]
        public UserRoles Role { get; set; }

        [Column("reset_password_token")]
        public string? ResetPasswordToken { get; set; }

        /// <summary>
        /// Reset password token's expiried date
        /// </summary>
        [Column("token_expired_date")]
        public DateTime? ExpiryDate { get; set; }

        [Column("reject_reason")]
        public string? RejectReason { get; set; }

        [Column("concurrency_stamp")]
        [Timestamp]
        public byte[] ConcurrencyStamp { get; set; } = null!;

        public ICollection<PublisherSource> AffiliateSources { get; set; } = new List<PublisherSource>();

        public ICollection<PublisherOffer> PublisherOffers { get; set; } = new List<PublisherOffer>();

        public ICollection<Transaction>? Transactions { get; set; }

        public ICollection<UserBank>? UserBanks { get; set; }

        // Navigation properties for one-to-one relationships
        public PublisherProfile PublisherProfile { get; set; } = null!;

        public AdvertiserProfile AdvertiserProfile { get; set; } = null!;

        public Wallet Wallet { get; set; } = null!;

        public ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();
    }
}
