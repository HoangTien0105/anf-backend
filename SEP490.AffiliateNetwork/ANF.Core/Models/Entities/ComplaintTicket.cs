using ANF.Core.Commons;

namespace ANF.Core.Models.Entities
{
    public class ComplaintTicket : IEntity
    {
        public long Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? PublisherCode { get; set; }

        public string? AdvertiserCode { get; set; }

        // TODO: Fix the property based on the requirement
        public string? ImageUrl { get; set; }

        public long? TypeId { get; set; }

        public ComplaintType? Type { get; set; }
    }
}
