using Modsen.Domain;

namespace Modsen.Domain
{
    public interface IEventRepository : IRepository<MyEvent>
    {
        Task<MyEvent?> GetEventWithImagesAsync(int eventId, CancellationToken cancellationToken);
        Task<IEnumerable<MyEvent>> GetEventsByCategoryAsync(string category, CancellationToken cancellationToken);
        Task<IEnumerable<MyEvent>> GetPagedEventsAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<List<string>> GetAllEventImagePathsAsync(CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task AddEventAsync(MyEvent newEvent, CancellationToken cancellationToken);
        Task<MyEvent> GetEventByIdAsync(int eventId, CancellationToken cancellationToken);
        Task DeleteEventAsync(MyEvent eventToDelete, CancellationToken cancellationToken);
        Task<bool> CheckEventExistsAsync(int eventId, CancellationToken cancellationToken);
        Task<IEnumerable<Member>> GetEventMembersAsync(int eventId, CancellationToken cancellationToken);
        Task<IEnumerable<MyEvent>> GetAllEventsAsync(CancellationToken cancellationToken);
        Task<MyEvent> GetEventByNameAsync(string name, CancellationToken cancellationToken);
        Task<IEnumerable<MyEvent>> GetEventsByCriteriaAsync(DateTime? date, string? location, string? category, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(int eventId, CancellationToken cancellationToken);
    }
}
