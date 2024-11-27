using Modsen.Domain;

namespace Modsen.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ModsenContext _context;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;

        public UnitOfWork(ModsenContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository 
        {
            get { return _userRepository ??= new UserRepository(_context); }
        }

        public IRoleRepository RoleRepository
        {
            get { return _roleRepository ??= new RoleRepository(_context); }
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}