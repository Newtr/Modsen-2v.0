using Modsen.Domain;

namespace Modsen.Domain
{
    public interface IEventRepository : IRepository<MyEvent>
    {
        Task<MyEvent?> GetEventWithImagesAsync(int eventId, CancellationToken cancellationToken);
        Task<IEnumerable<MyEvent>> GetEventsByCategoryAsync(string category, CancellationToken cancellationToken);
        Task<IEnumerable<MyEvent>> GetPagedEventsAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<List<string>> GetAllEventImagePathsAsync(CancellationToken cancellationToken);
    }
}
