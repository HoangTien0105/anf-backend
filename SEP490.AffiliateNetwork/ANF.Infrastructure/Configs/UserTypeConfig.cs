using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    public class UserTypeConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasAlternateKey(u => u.UserCode);
            builder.HasIndex(u => u.UserCode).IsUnique();
            builder.Property(u => u.Id).ValueGeneratedNever();
        }
    }
}
