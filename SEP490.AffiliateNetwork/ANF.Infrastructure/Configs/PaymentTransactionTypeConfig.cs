using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    public class PaymentTransactionTypeConfig : IEntityTypeConfiguration<PaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
        {
            builder.HasOne(pt => pt.User)
                .WithMany(u => u.PaymentTransactions)
                .HasForeignKey(pt => pt.UserCode)
                .HasPrincipalKey(u  => u.UserCode)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(pt => pt.Wallet)
                .WithMany(w => w.PaymentTransactions)
                .HasForeignKey(pt => pt.WalletId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
