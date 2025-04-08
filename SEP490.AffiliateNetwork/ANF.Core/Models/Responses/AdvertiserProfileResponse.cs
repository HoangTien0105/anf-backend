using ANF.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Responses
{
    public class AdvertiserProfileResponse
    {
        [Column("user_id")]
        public long Id { get; set; }

        [Column("user_code")]
        public string UserCode { get; set; } = null!;

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
        
        [Column("user_status")]
        public string? Status { get; set; }

        [Column("reject_reason")]
        public string? RejectReason { get; set; }

        public string? CompanyName { get; set; }

        public string? Industry { get; set; }

        public string? ImageUrl { get; set; }

        public string? Bio { get; set; }
    }
}
