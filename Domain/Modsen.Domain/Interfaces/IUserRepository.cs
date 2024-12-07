namespace Modsen.Domain
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
        Task<bool> AnyAsync(string email, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetUsersAsync(int page, int pageSize);
    }
}