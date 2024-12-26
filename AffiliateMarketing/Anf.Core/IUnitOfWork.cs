namespace Anf.Core
{
    /// <summary>
    /// Represents a unit of work that encapsulates a set of operations to be performed as a single transaction.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>The repository for the specified entity type.</returns>
        IRepository<T> GetRepository<T>() where T : class;

        /// <summary>
        /// Saves all changes made in this unit of work to the underlying data store.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the underlying data store.</returns>
        Task<int> SaveAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Discards all changes made in this unit of work.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous rollback operation.</returns>
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
