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

        [Column("concurrency_stamp")]
        [Timestamp]
        public byte[] ConcurrencyStamp { get; set; } = null!;

        public ICollection<PublisherSource> AffiliateSources { get; set; } = new List<PublisherSource>();

        public ICollection<SubPurchase> SubPurchases { get; set; } = new List<SubPurchase>();

        // TODO: Remove the comment when interacting with Offers table
        //public ICollection<PublisherOffer> PublisherOffers { get; set; } = new List<PublisherOffer>();

        // Navigation properties for one-to-one relationships
        public PublisherProfile PublisherProfile { get; set; } = null!;

        public AdvertiserProfile AdvertiserProfile { get; set; } = null!;
    }
}
