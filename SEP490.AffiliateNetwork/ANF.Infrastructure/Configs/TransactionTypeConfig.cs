using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    public class TransactionTypeConfig : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.HasOne(pt => pt.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(pt => pt.UserCode)
                .HasPrincipalKey(u  => u.UserCode)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(pt => pt.Wallet)
                .WithMany(w => w.Transactions)
                .HasForeignKey(pt => pt.WalletId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.Subscription)
                .WithMany(s => s.Transactions)
                .HasForeignKey(t => t.SubscriptionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
