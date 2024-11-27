using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using Modsen.Domain;
using Member = Modsen.Domain.Member;

namespace Modsen.Application
{
public class GetEventMembersUseCase
{
    public async Task<IEnumerable<Member>> ExecuteAsync(int eventId)
    {
        var eventExists = await CheckEventExistsAsync(eventId);
        if (!eventExists)
            throw new NotFoundException($"Событие с ID {eventId} не найдено.");

        return await FetchMembersByEventId(eventId)
            .AsNoTracking()
            .ToListAsync();
    }

    private Task<bool> CheckEventExistsAsync(int eventId) => Task.FromResult(true);
    private IQueryable<Member> FetchMembersByEventId(int eventId)
    {
        var members = new List<Member>();
        return members.AsQueryable();
    }
}

}