using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    public class CampaignTypeConfig : IEntityTypeConfiguration<Campaign>
    {
        public void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.HasOne(c => c.Advertiser)
                .WithMany(u => u.Campaigns)
                .HasForeignKey(c => c.AdvertiserCode)
                .HasPrincipalKey(u => u.UserCode)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
