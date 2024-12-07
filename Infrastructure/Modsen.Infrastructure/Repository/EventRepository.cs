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

    }
}
