using AutoMapper.Execution;
using Modsen.Domain;
using Member = Modsen.Domain.Member;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Modsen.Application
{
    public class GetEventMembersUseCase
{
    private readonly IEventRepository _eventRepository;

    public GetEventMembersUseCase(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
    }

    public async Task<IEnumerable<Member>> ExecuteAsync(int eventId, CancellationToken cancellationToken)
    {
        var eventExists = await _eventRepository.CheckEventExistsAsync(eventId, cancellationToken);
        if (!eventExists)
            throw new NotFoundException($"Событие с ID {eventId} не найдено.");

        return await _eventRepository.GetEventMembersAsync(eventId, cancellationToken);
    }
}
}
