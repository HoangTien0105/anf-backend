using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    public class TrackingValidationTypeConfig : IEntityTypeConfiguration<TrackingValidation>
    {
        public void Configure(EntityTypeBuilder<TrackingValidation> builder)
        {
            builder.HasOne(x => x.TrackingEvent)
                .WithOne(c => c.TrackingValidation)
                .HasForeignKey<TrackingValidation>(x => x.ClickId)
            .IsRequired(false);

            builder.HasIndex(f => f.ClickId).IsUnique();
        }
    }
}
