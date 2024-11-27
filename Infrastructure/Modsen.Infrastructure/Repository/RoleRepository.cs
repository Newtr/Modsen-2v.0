using Modsen.Domain;

namespace Modsen.Infrastructure
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ModsenContext context) : base(context) {}
    }
}
