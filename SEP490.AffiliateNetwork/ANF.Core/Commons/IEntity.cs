using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Commons
{
    /// <summary>
    /// Represents an entity using int as the primary key.
    /// </summary>
    public interface IEntity
    {
        [Key]
        long Id { get; set; }
    }
}
