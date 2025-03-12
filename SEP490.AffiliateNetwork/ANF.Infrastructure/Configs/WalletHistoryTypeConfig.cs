using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    internal class WalletHistoryTypeConfig : IEntityTypeConfiguration<WalletHistory>
    {
        public void Configure(EntityTypeBuilder<WalletHistory> builder)
        {
            builder.HasOne(wh => wh.Transaction)
                .WithOne(w => w.WalletHistory)
                .HasForeignKey<WalletHistory>(wh => wh.TransactionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
