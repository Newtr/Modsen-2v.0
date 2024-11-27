using Microsoft.EntityFrameworkCore;
using Modsen.Domain;

namespace Modsen.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly ModsenContext _context;

        public UserRepository(ModsenContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.Include(u => u.Role).ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
        }


        public async Task<User> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);
        }


        public async Task<IQueryable<User>> GetUsersAsync(int page, int pageSize)
        {
            return await Task.Run(() =>
                _context.Users
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .AsQueryable());
        }

        public async Task<bool> AnyAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }


        public async Task AddAsync(User entity, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(entity, cancellationToken);
        }


        public async Task UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
        }

        public async Task DeleteAsync(User entity)
        {
            _context.Users.Remove(entity);
        }
    }
}