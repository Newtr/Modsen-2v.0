using Modsen.Domain;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Modsen.Application
{
    public class MemberService
{
    private readonly GetEventMembersUseCase _getEventMembersUseCase;
    private readonly GetMemberByIdUseCase _getMemberByIdUseCase;
    private readonly RegisterMemberUseCase _registerMemberUseCase;
    private readonly UnregisterMemberUseCase _unregisterMemberUseCase;

    public MemberService(
        GetEventMembersUseCase getEventMembersUseCase,
        GetMemberByIdUseCase getMemberByIdUseCase,
        RegisterMemberUseCase registerMemberUseCase,
        UnregisterMemberUseCase unregisterMemberUseCase)
    {
        _getEventMembersUseCase = getEventMembersUseCase;
        _getMemberByIdUseCase = getMemberByIdUseCase;
        _registerMemberUseCase = registerMemberUseCase;
        _unregisterMemberUseCase = unregisterMemberUseCase;
    }

    public Task<IEnumerable<Member>> GetEventMembersAsync(int eventId) =>
        _getEventMembersUseCase.ExecuteAsync(eventId);

    public Task<Member> GetMemberByIdAsync(int memberId) =>
        _getMemberByIdUseCase.ExecuteAsync(memberId);

    public Task<string> RegisterMemberAsync(int eventId, int memberId) =>
        _registerMemberUseCase.ExecuteAsync(eventId, memberId);

    public Task<string> UnregisterMemberAsync(int eventId, int memberId) =>
        _unregisterMemberUseCase.ExecuteAsync(eventId, memberId);
}
}
