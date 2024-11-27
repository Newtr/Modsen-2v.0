namespace Modsen.Domain
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByRefreshTokenAsync(string refreshToken);
        Task<bool> AnyAsync(string email);
        Task<IQueryable<User>> GetUsersAsync(int page, int pageSize);
    }
}