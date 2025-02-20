using ANF.Core;

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

        //METHOD IS NOT IMPLEMENTED
        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            var affectedRows = await _dbContext.SaveChangesAsync(cancellationToken);
            return affectedRows;
        }
    }
}
