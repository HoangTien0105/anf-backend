using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    public class PostbackDataTypeConfig : IEntityTypeConfiguration<PostbackData>
    {
        public void Configure(EntityTypeBuilder<PostbackData> builder)
        {
            builder.HasOne(pd => pd.Offer)
                .WithMany(o => o.PostbackData)
                .HasForeignKey(pd => pd.OfferId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
