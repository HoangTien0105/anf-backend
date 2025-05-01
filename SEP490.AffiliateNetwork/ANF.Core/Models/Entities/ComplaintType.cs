using ANF.Core.Commons;

namespace ANF.Core.Models.Entities
{
    public class ComplaintType : IEntity
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<ComplaintTicket> ComplaintTickets { get; set; } = [];
    }
}
