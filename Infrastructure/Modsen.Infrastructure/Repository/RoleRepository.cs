using System.Data.Entity;
using Modsen.Domain;

namespace Modsen.Infrastructure
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ModsenContext _context;

        public RoleRepository(ModsenContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task AddAsync(Role entity)
        {
            await _context.Roles.AddAsync(entity);
        }

        public async Task UpdateAsync(Role entity)
        {
            _context.Roles.Update(entity);
        }

        public async Task DeleteAsync(Role entity)
        {
            _context.Roles.Remove(entity);
        }
    }
}