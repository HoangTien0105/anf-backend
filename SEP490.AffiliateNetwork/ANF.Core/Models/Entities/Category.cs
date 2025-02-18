using ANF.Core.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace ANF.Core.Models.Entities
{
    /// <summary>
    /// Information about a category (for campaign filter)
    /// </summary>
    public class Category : IEntity
    {
        [Column("cate_id")]
        public long Id { get; set; }

        [Column("cate_name")]
        public string Name { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }

        public ICollection<Category>? Categories { get; set; }
    }
}
