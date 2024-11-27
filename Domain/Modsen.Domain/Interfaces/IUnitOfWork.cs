namespace Modsen.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }

        Task<int> CommitAsync(CancellationToken cancellationToken);
    }
}