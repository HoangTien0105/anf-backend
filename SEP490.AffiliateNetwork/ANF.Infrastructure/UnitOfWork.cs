using ANF.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ANF.Infrastructure
{
    public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.TryGetValue(typeof(T), out var repository))
            {
                return (IRepository<T>)repository;
            }

            var newRepository = new Repository<T>(_dbContext);
            _repositories[typeof(T)] = newRepository;
            return newRepository;
        }
        
        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // Check if there's an active transaction
                if (_dbContext.Database.CurrentTransaction != null)
                {
                    await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
                }

                // Reset all tracked changes in the DbContext
                var entries = _dbContext.ChangeTracker.Entries()
                    .Where(e => e.State != EntityState.Unchanged)
                    .ToList();

                foreach (var entry in entries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.State = EntityState.Detached; // Remove added entities
                            break;
                        case EntityState.Modified:
                        case EntityState.Deleted:
                            await entry.ReloadAsync(cancellationToken); // Revert to original values
                            break;
                    }
                }
            }
            catch (Exception)
            {
                // If something goes wrong, ensure we don't leave the context in a bad state
                _dbContext.ChangeTracker.Clear(); // Last resort: clear all tracking
                throw; // Re-throw to let the caller handle it
            }
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            var affectedRows = await _dbContext.SaveChangesAsync(cancellationToken);
            return affectedRows;
        }
    }
}
