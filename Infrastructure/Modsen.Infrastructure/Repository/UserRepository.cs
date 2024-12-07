using Microsoft.EntityFrameworkCore;
using Modsen.Domain;

namespace Modsen.Infrastructure
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ModsenContext context) : base(context) {}

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(u => u.Role) 
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
        public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);
        }

        public async Task<bool> AnyAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await GetPagedAsync(page, pageSize, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(int page, int pageSize)
        {
            return await _dbSet.AsNoTracking()
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
        }

    }
}
