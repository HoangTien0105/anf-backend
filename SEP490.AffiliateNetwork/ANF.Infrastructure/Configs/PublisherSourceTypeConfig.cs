using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    public class PublisherSourceTypeConfig : IEntityTypeConfiguration<PublisherSource>
    {
        public void Configure(EntityTypeBuilder<PublisherSource> builder)
        {
            builder.HasOne(p => p.Publisher)
                .WithMany(c => c.AffiliateSources)
                .HasForeignKey(p => p.PublisherId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
