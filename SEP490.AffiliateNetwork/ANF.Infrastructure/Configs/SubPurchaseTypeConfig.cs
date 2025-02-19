using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    public class SubPurchaseTypeConfig : IEntityTypeConfiguration<SubPurchase>
    {
        public void Configure(EntityTypeBuilder<SubPurchase> builder)
        {
            builder.HasOne(p => p.Subscription)
                .WithMany(c => c.SubPurchases)
                .HasForeignKey(p => p.SubscriptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.Advertiser)
                .WithMany(c => c.SubPurchases)
                .HasForeignKey(p => p.AdvertiserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
