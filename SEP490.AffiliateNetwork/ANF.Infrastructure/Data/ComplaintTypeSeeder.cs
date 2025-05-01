using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ANF.Infrastructure.Data
{
    public static class ComplaintTypeSeeder
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComplaintType>().HasData(
                new ComplaintType
                {
                    Id = 1,
                    Name = "Misleading Affiliate Links",
                    Description = "Affiliate links are disguised or not properly disclosed to users."
                },
                new ComplaintType
                {
                    Id = 2,
                    Name = "Commission Tracking Error",
                    Description = "Affiliate sales or clicks are not being properly tracked or credited."
                },
                new ComplaintType
                {
                    Id = 3,
                    Name = "False Product Claims",
                    Description = "Affiliate promotions contain inaccurate or exaggerated product claims."
                },
                new ComplaintType
                {
                    Id = 4,
                    Name = "Late Commission Payment",
                    Description = "Delayed or missing commission payments for confirmed sales."
                },
                new ComplaintType
                {
                    Id = 5,
                    Name = "Unauthorized Link Usage",
                    Description = "Affiliate links being used without proper authorization or agreement."
                },
                new ComplaintType
                {
                    Id = 6,
                    Name = "Invalid Referral Data",
                    Description = "Referral data showing inconsistencies or errors in tracking system."
                },
                new ComplaintType
                {
                    Id = 7,
                    Name = "Compliance Violation",
                    Description = "Affiliate marketing practices violating program terms or regulations."
                },
                new ComplaintType
                {
                    Id = 8,
                    Name = "Commission Rate Dispute",
                    Description = "Disagreement over applied commission rates or tier calculations."
                },
                new ComplaintType
                {
                    Id = 9,
                    Name = "Cookie Duration Issue",
                    Description = "Problems with affiliate cookie tracking duration or attribution."
                },
                new ComplaintType
                {
                    Id = 10,
                    Name = "Program Communication",
                    Description = "Lack of or unclear communication about program changes or updates."
                }
            );
        }

    }
}
