namespace ANF.Core.Commons
{
    /// <summary>
    /// Represents an entity using int as the primary key.
    /// </summary>
    public interface IEntity
    {
        int Id { get; set; }
    }
}
