namespace Modsen.Domain
{
public interface IMemberRepository
{
    Task<Member?> GetMemberByIdAsync(int memberId, CancellationToken cancellationToken);
    Task<Member?> GetMemberByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(int memberId, CancellationToken cancellationToken);
    Task<bool> IsMemberRegisteredAsync(int eventId, int memberId, CancellationToken cancellationToken);
    Task RegisterMemberToEventAsync(int eventId, int memberId, CancellationToken cancellationToken);
    Task UnregisterMemberFromEventAsync(int eventId, int memberId, CancellationToken cancellationToken);
}
}