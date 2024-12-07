using Microsoft.EntityFrameworkCore;
using Modsen.Domain;
using System.Linq.Expressions;

namespace Modsen.Infrastructure
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }


        public async Task<IEnumerable<T>> IncludeAsync(Expression<Func<T, object>> includeExpression)
        {
            return await _dbSet.Include(includeExpression).ToListAsync();
        }

    }
}
