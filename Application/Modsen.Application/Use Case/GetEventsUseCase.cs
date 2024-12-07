using Modsen.Domain;

namespace Modsen.Application
{
public class GetEventsUseCase
{
    private readonly IEventRepository _eventRepository;

    public GetEventsUseCase(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
    }

    public Task<IEnumerable<MyEvent>> GetAllEventsAsync(CancellationToken cancellationToken)
    {
        return _eventRepository.GetAllEventsAsync(cancellationToken);
    }

    public Task<MyEvent> GetEventByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _eventRepository.GetEventByIdAsync(id, cancellationToken);
    }

    public Task<MyEvent> GetEventByNameAsync(string name, CancellationToken cancellationToken)
    {
        return _eventRepository.GetEventByNameAsync(name, cancellationToken);
    }

    public Task<IEnumerable<MyEvent>> GetEventsByCriteriaAsync(DateTime? date, string? location, string? category, CancellationToken cancellationToken)
    {
        return _eventRepository.GetEventsByCriteriaAsync(date, location, category, cancellationToken);
    }
}

}
