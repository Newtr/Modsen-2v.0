using Microsoft.EntityFrameworkCore;
using Modsen.Domain;
using Modsen.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Modsen.Application
{
    public class RegisterMemberUseCase
{
    private readonly ModsenContext _context;

    public RegisterMemberUseCase(ModsenContext context)
    {
        _context = context;
    }

    public async Task<string> ExecuteAsync(int eventId, int memberId, CancellationToken cancellationToken)
    {
        var eventExists = await CheckEventExistsAsync(eventId, cancellationToken);
        if (!eventExists)
            throw new NotFoundException($"Событие с ID {eventId} не найдено.");

        var memberExists = await CheckMemberExistsAsync(memberId, cancellationToken);
        if (!memberExists)
            throw new NotFoundException($"Участник с ID {memberId} не найден.");

        var alreadyRegistered = await IsMemberRegisteredAsync(eventId, memberId, cancellationToken);
        if (alreadyRegistered)
            throw new BadRequestException($"Участник с ID {memberId} уже зарегистрирован на событие с ID {eventId}.");

        await RegisterMemberToEvent(eventId, memberId, cancellationToken);
        return $"Участник с ID {memberId} успешно зарегистрирован на событие с ID {eventId}.";
    }

    private async Task<bool> CheckEventExistsAsync(int eventId, CancellationToken cancellationToken)
    {
        return await _context.Set<MyEvent>().AnyAsync(e => e.Id == eventId, cancellationToken);
    }

    private async Task<bool> CheckMemberExistsAsync(int memberId, CancellationToken cancellationToken)
    {
        return await _context.Set<Member>().AnyAsync(m => m.Id == memberId, cancellationToken);
    }

    private async Task<bool> IsMemberRegisteredAsync(int eventId, int memberId, CancellationToken cancellationToken)
    {
        return await _context.Set<Dictionary<string, object>>("EventsAndMembers")
            .AnyAsync(eam => (int)eam["EventID"] == eventId && (int)eam["MemberID"] == memberId, cancellationToken);
    }

    private async Task RegisterMemberToEvent(int eventId, int memberId, CancellationToken cancellationToken)
    {
        var newEventMember = new Dictionary<string, object>
        {
            { "EventID", eventId },
            { "MemberID", memberId }
        };

        _context.Set<Dictionary<string, object>>("EventsAndMembers").Add(newEventMember);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

}
