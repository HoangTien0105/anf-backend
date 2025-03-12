using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    public class UserBankTypeConfig : IEntityTypeConfiguration<UserBank>
    {
        public void Configure(EntityTypeBuilder<UserBank> builder)
        {
            builder.HasOne(ub => ub.User)
                .WithMany(u => u.UserBanks)
                .HasForeignKey(ub => ub.UserCode)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
