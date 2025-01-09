using Microsoft.AspNetCore.Identity;

namespace ANF.Core.Models.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = null!;

        public string? Address { get; set; }

        public string CitizenCode { get; set; } = null!;
    }
}
