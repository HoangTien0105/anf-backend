using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ANF.Infrastructure.Data
{
    public static class UserSeeder
    {
        public static void SeedDataForUsers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 100,
                    FirstName = "John",
                    LastName = "Smith",
                    PhoneNumber = "555-0123",
                    CitizenId = "JS123456789",
                    Email = "john.smith@email.com",
                    Password = "hashed_password_1",
                    EmailConfirmed = true,
                    Role = Core.Enums.UserRoles.Publisher,
                    Status = Core.Enums.UserStatus.Active,
                },
                new User
                {
                    Id = 101,
                    FirstName = "Sarah",
                    LastName = "Johnson",
                    PhoneNumber = "555-0124",
                    CitizenId = "SJ987654321",
                    Email = "sarah.j@email.com",
                    Password = "hashed_password_2",
                    EmailConfirmed = true,
                    Role = Core.Enums.UserRoles.Advertiser,
                    Status = Core.Enums.UserStatus.Active,
                },
                new User
                {
                    Id = 103,
                    Email = "saffiliatenetwork@gmail.com",
                    Password = "superstrongpassword123@",
                    EmailConfirmed = true,
                    Role = Core.Enums.UserRoles.Admin,
                    Status = Core.Enums.UserStatus.Active,
                }
            );
        }
    }
}
