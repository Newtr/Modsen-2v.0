using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Modsen.Domain;
using Modsen.Infrastructure;

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

        public async Task<IEnumerable<MyEvent>> GetAllEventsAsync()
        {
            return await _getEventsUseCase.GetAllEventsAsync();
        }

        public async Task<MyEvent> GetEventByIdAsync(int id)
        {
            return await _getEventsUseCase.GetEventByIdAsync(id);
        }

        public async Task<MyEvent> GetEventByNameAsync(string name)
        {
            return await _getEventsUseCase.GetEventByNameAsync(name);
        }

        public async Task<IEnumerable<MyEvent>> GetEventsByCriteriaAsync(DateTime? date, string? location, string? category)
        {
            return await _getEventsUseCase.GetEventsByCriteriaAsync(date, location, category);
        }

        public async Task<MyEvent> CreateEventAsync(MyEvent newEvent, List<IFormFile> eventImages, IWebHostEnvironment hostEnvironment)
        {
            return await _createEventUseCase.CreateEventAsync(newEvent, eventImages, hostEnvironment);
        }

        public async Task AddImagesToEventAsync(int eventId, List<IFormFile> eventImages, IWebHostEnvironment hostEnvironment)
        {
            await _addImagesToEventUseCase.AddImagesToEventAsync(eventId, eventImages, hostEnvironment);
        }

        public async Task UpdateEventAsync(int eventId, MyEvent updatedEvent, List<IFormFile> eventImages, string userEmail, IWebHostEnvironment hostEnvironment)
        {
            await _updateEventUseCase.UpdateEventAsync(eventId, updatedEvent, eventImages, userEmail, hostEnvironment);
        }

        public async Task DeleteEventAsync(int eventId, IWebHostEnvironment hostEnvironment)
        {
            await _deleteEventUseCase.DeleteEventAsync(eventId, hostEnvironment);
        }
    }
}
