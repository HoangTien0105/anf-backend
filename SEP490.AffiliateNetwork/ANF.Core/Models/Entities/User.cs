using ANF.Core.Commons;
using ANF.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Models.Entities
{
    public class User : IGuidEntity
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string CitizenNo { get; set; } = null!;

        public string? Address { get; set; }

        public DateTime? Birthday { get; set; }

        public string? Image { get; set; }

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        [Timestamp]
        public byte[]? ConcurrencyStamp { get; set; }

        public bool? EmailConfirmed { get; set; }

        public string? Bio { get; set; }

        public UserRoles Role { get; set; }
    }
}
