using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    public class AdvertiserProfileTypeConfig : IEntityTypeConfiguration<AdvertiserProfile>
    {
        public void Configure(EntityTypeBuilder<AdvertiserProfile> builder)
        {
            builder.HasOne(p => p.Advertiser)
                .WithOne(c => c.AdvertiserProfile)
                .HasForeignKey<AdvertiserProfile>(p => p.AdvertiserId)
                .IsRequired(false);

            builder.HasIndex(p => p.AdvertiserId)
                .IsUnique();
        }
    }
}
