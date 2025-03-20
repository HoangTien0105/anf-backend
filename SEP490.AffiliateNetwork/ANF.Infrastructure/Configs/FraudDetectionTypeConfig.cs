using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    public class FraudDetectionTypeConfig : IEntityTypeConfiguration<FraudDetection>
    {
        public void Configure(EntityTypeBuilder<FraudDetection> builder)
        {
            builder.HasOne(x => x.TrackingEvent)
                .WithOne(c => c.FraudDetection)
                .HasForeignKey<FraudDetection>(x => x.ClickId)
            .IsRequired(false);

            builder.HasIndex(f => f.ClickId).IsUnique();
        }
    }
}
