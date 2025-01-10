using ANF.Core;
using Microsoft.EntityFrameworkCore;

namespace ANF.Infrastructure
{
    public class Repository<T>(ApplicationDbContext dbContext) : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly DbSet<T> _dbSet = dbContext.Set<T>();

        public void Add(T entity)
        {
            _dbContext.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbContext.AddRange(entities);
        }

        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbContext.RemoveRange(entities);
        }

        public async Task<T?> FindByIdAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public void Update(T entity)
        {
            _dbContext.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbContext.UpdateRange(entities);
        }
    }
}
