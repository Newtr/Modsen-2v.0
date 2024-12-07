using Modsen.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Modsen.Application
{
public class RegisterMemberUseCase
{
    private readonly IEventRepository _eventRepository;
    private readonly IMemberRepository _memberRepository;

    public RegisterMemberUseCase(IEventRepository eventRepository, IMemberRepository memberRepository)
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

        var alreadyRegistered = await _memberRepository.IsMemberRegisteredAsync(eventId, memberId, cancellationToken);
        if (alreadyRegistered)
            throw new BadRequestException($"Участник с ID {memberId} уже зарегистрирован на событие с ID {eventId}.");

        await _memberRepository.RegisterMemberToEventAsync(eventId, memberId, cancellationToken);

        return $"Участник с ID {memberId} успешно зарегистрирован на событие с ID {eventId}.";
    }
}

}
