using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using Modsen.Domain;
using Member = Modsen.Domain.Member;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Modsen.Infrastructure;

namespace Modsen.Application
{
    public class GetEventMembersUseCase
{
    private readonly ModsenContext _context;

    public GetEventMembersUseCase(ModsenContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Member>> ExecuteAsync(int eventId, CancellationToken cancellationToken)
    {
        var eventExists = await CheckEventExistsAsync(eventId, cancellationToken);
        if (!eventExists)
            throw new NotFoundException($"Событие с ID {eventId} не найдено.");

        return await FetchMembersByEventId(eventId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    private async Task<bool> CheckEventExistsAsync(int eventId, CancellationToken cancellationToken)
    {
        return await _context.Set<MyEvent>()
            .AnyAsync(e => e.Id == eventId, cancellationToken);
    }

    private IQueryable<Member> FetchMembersByEventId(int eventId)
    {
        return _context.Set<Dictionary<string, object>>("EventsAndMembers")
            .Where(eam => (int)eam["EventID"] == eventId)
            .Join(
                _context.Set<Member>(), 
                eam => (int)eam["MemberID"], 
                member => member.Id, 
                (eam, member) => member 
            )
            .AsQueryable();
    }
}

}
