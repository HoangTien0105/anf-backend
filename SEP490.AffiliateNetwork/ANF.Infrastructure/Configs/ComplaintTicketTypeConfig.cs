using ANF.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ANF.Infrastructure.Configs
{
    public class ComplaintTicketTypeConfig : IEntityTypeConfiguration<ComplaintTicket>
    {
        public void Configure(EntityTypeBuilder<ComplaintTicket> builder)
        {
            builder.HasOne(x => x.Type)
                .WithMany(y => y.ComplaintTickets)
                .HasForeignKey(x => x.TypeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
