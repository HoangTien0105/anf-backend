using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    public class TrackingEventTypeConfig : IEntityTypeConfiguration<TrackingEvent>
    {
        public void Configure(EntityTypeBuilder<TrackingEvent> builder)
        {
            builder.HasIndex(x => new { x.PublisherCode, x.OfferId })
                .IsUnique();

            builder.HasOne(x => x.Offer)
                .WithMany(y => y.TrackingEvents)
                .HasForeignKey(x => x.OfferId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
