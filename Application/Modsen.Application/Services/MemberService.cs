using Modsen.Domain;
using System.Collections.Generic;
using System.Threading;
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

        public Task<IEnumerable<Member>> GetEventMembersAsync(int eventId, CancellationToken cancellationToken) =>
            _getEventMembersUseCase.ExecuteAsync(eventId, cancellationToken);

        public Task<Member> GetMemberByIdAsync(int memberId, CancellationToken cancellationToken) =>
            _getMemberByIdUseCase.ExecuteAsync(memberId, cancellationToken);

        public Task<string> RegisterMemberAsync(int eventId, int memberId, CancellationToken cancellationToken) =>
            _registerMemberUseCase.ExecuteAsync(eventId, memberId, cancellationToken);

        public Task<string> UnregisterMemberAsync(int eventId, int memberId, CancellationToken cancellationToken) =>
            _unregisterMemberUseCase.ExecuteAsync(eventId, memberId, cancellationToken);
    }
}
