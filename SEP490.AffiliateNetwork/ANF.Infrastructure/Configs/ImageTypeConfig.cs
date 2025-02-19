using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    public class ImageTypeConfig : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasOne(p => p.Offer)
                .WithMany(c => c.Images)
                .HasForeignKey(p => p.OfferId)
                //.IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Campaign)
                .WithMany(c => c.Images)
                .HasForeignKey(p => p.CampaignId)
                //.IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
