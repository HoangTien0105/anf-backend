using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    public class CampaignImageTypeConfig : IEntityTypeConfiguration<CampaignImage>
    {
        public void Configure(EntityTypeBuilder<CampaignImage> builder)
        {
            builder.HasOne(p => p.Campaign)
                .WithMany(c => c.Images)
                .HasForeignKey(p => p.CampaignId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
