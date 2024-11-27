using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Modsen.Domain;
using Modsen.Infrastructure;
using System.Threading;

namespace Modsen.Application
{
    public class EventService
    {
        private readonly GetEventsUseCase _getEventsUseCase;
        private readonly CreateEventUseCase _createEventUseCase;
        private readonly UpdateEventUseCase _updateEventUseCase;
        private readonly DeleteEventUseCase _deleteEventUseCase;
        private readonly AddImagesToEventUseCase _addImagesToEventUseCase;

        public EventService(
            GetEventsUseCase getEventsUseCase,
            CreateEventUseCase createEventUseCase,
            UpdateEventUseCase updateEventUseCase,
            DeleteEventUseCase deleteEventUseCase,
            AddImagesToEventUseCase addImagesToEventUseCase)
        {
            _getEventsUseCase = getEventsUseCase;
            _createEventUseCase = createEventUseCase;
            _updateEventUseCase = updateEventUseCase;
            _deleteEventUseCase = deleteEventUseCase;
            _addImagesToEventUseCase = addImagesToEventUseCase;
        }

        public async Task<IEnumerable<MyEvent>> GetAllEventsAsync(CancellationToken cancellationToken)
        {
            return await _getEventsUseCase.GetAllEventsAsync(cancellationToken);
        }

        public async Task<MyEvent> GetEventByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _getEventsUseCase.GetEventByIdAsync(id, cancellationToken);
        }

        public async Task<MyEvent> GetEventByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _getEventsUseCase.GetEventByNameAsync(name, cancellationToken);
        }

        public async Task<IEnumerable<MyEvent>> GetEventsByCriteriaAsync(DateTime? date, string? location, string? category, CancellationToken cancellationToken)
        {
            return await _getEventsUseCase.GetEventsByCriteriaAsync(date, location, category, cancellationToken);
        }

        public async Task<MyEvent> CreateEventAsync(MyEvent newEvent, List<IFormFile> eventImages, IWebHostEnvironment hostEnvironment, CancellationToken cancellationToken)
        {
            return await _createEventUseCase.CreateEventAsync(newEvent, eventImages, hostEnvironment, cancellationToken);
        }

        public async Task AddImagesToEventAsync(int eventId, List<IFormFile> eventImages, IWebHostEnvironment hostEnvironment, CancellationToken cancellationToken)
        {
            await _addImagesToEventUseCase.AddImagesToEventAsync(eventId, eventImages, hostEnvironment, cancellationToken);
        }

        public async Task UpdateEventAsync(int eventId, MyEvent updatedEvent, List<IFormFile> eventImages, string userEmail, IWebHostEnvironment hostEnvironment, CancellationToken cancellationToken)
        {
            await _updateEventUseCase.UpdateEventAsync(eventId, updatedEvent, eventImages, userEmail, hostEnvironment, cancellationToken);
        }

        public async Task DeleteEventAsync(int eventId, IWebHostEnvironment hostEnvironment, CancellationToken cancellationToken)
        {
            await _deleteEventUseCase.DeleteEventAsync(eventId, hostEnvironment, cancellationToken);
        }
    }
}
