using ANF.Core;

namespace ANF.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
