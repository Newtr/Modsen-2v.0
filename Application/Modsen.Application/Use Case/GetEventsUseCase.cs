using Microsoft.EntityFrameworkCore;
using Modsen.Domain;
using Modsen.Infrastructure;

namespace Modsen.Application
{
    public class GetEventsUseCase
    {
    private readonly ModsenContext _context;

    public GetEventsUseCase(ModsenContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MyEvent>> GetAllEventsAsync()
    {
        return await _context.Events
            .AsNoTracking()
            .Include(e => e.EventMembers)
            .ThenInclude(em => em.MemberEvents)
            .ToListAsync();
    }

    public async Task<MyEvent> GetEventByIdAsync(int id)
    {
        var myEvent = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
        if (myEvent == null)
            throw new NotFoundException("Event with the given ID was not found.");

        return myEvent;
    }

    public async Task<MyEvent> GetEventByNameAsync(string name)
    {
        var myEvent = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name);
        if (myEvent == null)
            throw new NotFoundException($"Event with the name '{name}' was not found.");

        return myEvent;
    }

    public async Task<IEnumerable<MyEvent>> GetEventsByCriteriaAsync(DateTime? date, string? location, string? category)
    {
        var query = _context.Events.AsNoTracking().AsQueryable();

        if (date.HasValue)
            query = query.Where(e => e.DateOfEvent.Date == date.Value.Date);
        if (!string.IsNullOrEmpty(location))
            query = query.Where(e => e.EventLocation.Contains(location));
        if (!string.IsNullOrEmpty(category))
            query = query.Where(e => e.EventCategory.Contains(category));

        return await query.ToListAsync();
    }
    }
}