using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace ANF.Infrastructure.Configs
{
    public class PublisherProfileTypeConfig : IEntityTypeConfiguration<PublisherProfile>
    {
        public void Configure(EntityTypeBuilder<PublisherProfile> builder)
        {
            builder.HasOne(p => p.Publisher)
                .WithOne(c => c.PublisherProfile)
                .HasForeignKey<PublisherProfile>(p => p.PublisherId)
            .IsRequired(false);

            builder.HasIndex(p => p.PublisherId)
                .IsUnique();
        }
    }
}
