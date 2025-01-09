namespace ANF.Core
{
    /// <summary>
    /// Defines the basic operations for a repository.
    /// </summary>
    /// <typeparam name="T">The type of entity the repository manages.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        void Add(T entity);

        /// <summary>
        /// Adds a range of new entities to the repository.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Retrieves all entities from the repository.
        /// </summary>
        /// <returns>An <see cref="IQueryable{T}"/> representing all entities.</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Finds an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="keyValues">The values of the keys to find the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity found or null.</returns>
        Task<T?> FindByIdAsync(params object[] keyValues);

        /// <summary>
        /// Deletes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(T entity);

        /// <summary>
        /// Deletes a range of entities from the repository.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        void DeleteRange(IEnumerable<T> entities);

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(T entity);

        /// <summary>
        /// Updates a range of existing entities in the repository.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        void UpdateRange(IEnumerable<T> entities);
    }
}
