using Microsoft.EntityFrameworkCore;
using Modsen.Domain;

namespace Modsen.Infrastructure
{
    public class EventRepository : GenericRepository<MyEvent>, IEventRepository
    {
            private readonly ModsenContext _context;

            public EventRepository(ModsenContext context) : base(context)
            {
                _context = context;
            }

        public async Task<MyEvent?> GetEventWithImagesAsync(int eventId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(e => e.EventImages)
                .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
        }

        public async Task<IEnumerable<MyEvent>> GetEventsByCategoryAsync(string category, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(e => e.EventCategory == category)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MyEvent>> GetPagedEventsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<string>> GetAllEventImagePathsAsync(CancellationToken cancellationToken)
        {
            return await _context.EventImages
                .AsNoTracking()
                .Select(ei => ei.ImagePath)
                .ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddEventAsync(MyEvent newEvent, CancellationToken cancellationToken)
        {
            await _context.Events.AddAsync(newEvent, cancellationToken);
        }

        public async Task<MyEvent> GetEventByIdAsync(int eventId, CancellationToken cancellationToken)
        {
            return await _context.Events
                .Include(e => e.EventImages)
                .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
        }

        public async Task DeleteEventAsync(MyEvent eventToDelete, CancellationToken cancellationToken)
        {
            _context.EventImages.RemoveRange(eventToDelete.EventImages);
            _context.Events.Remove(eventToDelete);

            await SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> CheckEventExistsAsync(int eventId, CancellationToken cancellationToken)
        {
            return await _context.Set<MyEvent>()
                .AnyAsync(e => e.Id == eventId, cancellationToken);
        }

        public async Task<IEnumerable<Member>> GetEventMembersAsync(int eventId, CancellationToken cancellationToken)
        {
            return await _context.Set<Dictionary<string, object>>("EventsAndMembers")
                .Where(eam => (int)eam["EventID"] == eventId)
                .Join(
                    _context.Set<Member>(), 
                    eam => (int)eam["MemberID"], 
                    member => member.Id, 
                    (eam, member) => member
                )
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MyEvent>> GetAllEventsAsync(CancellationToken cancellationToken)
        {
            return await _context.Events
                .AsNoTracking()
                .Include(e => e.EventMembers) 
                .Include(e => e.EventImages) 
                .ToListAsync(cancellationToken);
        }

        public async Task<MyEvent> GetEventByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Name == name, cancellationToken);
        }

        public async Task<IEnumerable<MyEvent>> GetEventsByCriteriaAsync(DateTime? date, string? location, string? category, CancellationToken cancellationToken)
        {
            var query = _context.Events.AsNoTracking().AsQueryable();

            if (date.HasValue)
                query = query.Where(e => e.DateOfEvent.Date == date.Value.Date);
            if (!string.IsNullOrEmpty(location))
                query = query.Where(e => e.EventLocation.Contains(location));
            if (!string.IsNullOrEmpty(category))
                query = query.Where(e => e.EventCategory.Contains(category));

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int eventId, CancellationToken cancellationToken)
        {
            return await _context.Set<MyEvent>().AnyAsync(e => e.Id == eventId, cancellationToken);
        }

        public async Task UpdateAsync(MyEvent entity, CancellationToken cancellationToken)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Set<MyEvent>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
