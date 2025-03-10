using System.ComponentModel.DataAnnotations;

namespace ANF.Core.Commons
{
    /// <summary>
    /// Interface for entities that have a Guid as their primary key.
    /// </summary>
    public interface IGuidEntity
    {
        [Key]
        Guid Id { get; set; }
    }
}
