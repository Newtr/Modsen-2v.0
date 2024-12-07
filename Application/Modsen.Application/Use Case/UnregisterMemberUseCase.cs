using Modsen.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Modsen.Application
{
public class UnregisterMemberUseCase
{
    private readonly IEventRepository _eventRepository;
    private readonly IMemberRepository _memberRepository;

    public UnregisterMemberUseCase(IEventRepository eventRepository, IMemberRepository memberRepository)
    {
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
    }

    public async Task<string> ExecuteAsync(int eventId, int memberId, CancellationToken cancellationToken)
    {
        var eventExists = await _eventRepository.ExistsAsync(eventId, cancellationToken);
        if (!eventExists)
            throw new NotFoundException($"Событие с ID {eventId} не найдено.");

        var memberExists = await _memberRepository.ExistsAsync(memberId, cancellationToken);
        if (!memberExists)
            throw new NotFoundException($"Участник с ID {memberId} не найден.");

        var isRegistered = await _memberRepository.IsMemberRegisteredAsync(eventId, memberId, cancellationToken);
        if (!isRegistered)
            throw new BadRequestException($"Участник с ID {memberId} не зарегистрирован на событие с ID {eventId}.");

        await _memberRepository.UnregisterMemberFromEventAsync(eventId, memberId, cancellationToken);

        return $"Участник с ID {memberId} успешно удален из события с ID {eventId}.";
    }
}

}
