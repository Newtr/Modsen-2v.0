using Microsoft.EntityFrameworkCore;
using Modsen.Domain;

namespace Modsen.Infrastructure
{
public class MemberRepository : IMemberRepository
{
    private readonly ModsenContext _context;

    public MemberRepository(ModsenContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Member?> GetMemberByIdAsync(int memberId, CancellationToken cancellationToken)
    {
        return await _context.Set<Member>()
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == memberId, cancellationToken);
    }
    public async Task<Member?> GetMemberByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.Set<Member>()
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == userId, cancellationToken);
    }

    public async Task<bool> ExistsAsync(int memberId, CancellationToken cancellationToken)
    {
        return await _context.Set<Member>().AnyAsync(m => m.Id == memberId, cancellationToken);
    }

    public async Task<bool> IsMemberRegisteredAsync(int eventId, int memberId, CancellationToken cancellationToken)
    {
        return await _context.Set<Dictionary<string, object>>("EventsAndMembers")
            .AnyAsync(eam => (int)eam["EventID"] == eventId && (int)eam["MemberID"] == memberId, cancellationToken);
    }

    public async Task RegisterMemberToEventAsync(int eventId, int memberId, CancellationToken cancellationToken)
    {
        var newEventMember = new Dictionary<string, object>
        {
            { "EventID", eventId },
            { "MemberID", memberId }
        };

        _context.Set<Dictionary<string, object>>("EventsAndMembers").Add(newEventMember);
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task UnregisterMemberFromEventAsync(int eventId, int memberId, CancellationToken cancellationToken)
    {
        var eventMember = await _context.Set<Dictionary<string, object>>("EventsAndMembers")
            .FirstOrDefaultAsync(eam => (int)eam["EventID"] == eventId && (int)eam["MemberID"] == memberId, cancellationToken);

        if (eventMember == null)
            throw new BadRequestException($"Запись о событии с ID {eventId} и участнике с ID {memberId} не найдена.");

        _context.Set<Dictionary<string, object>>("EventsAndMembers").Remove(eventMember);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
}