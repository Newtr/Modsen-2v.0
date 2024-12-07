using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Modsen.Domain;

namespace Modsen.Application
{
    public class CreateEventUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly ImageService _imageService;

        public CreateEventUseCase(IEventRepository eventRepository, ImageService imageService)
        {
            _eventRepository = eventRepository;
            _imageService = imageService;
        }

        public async Task<MyEvent> CreateEventAsync(
            MyEvent newEvent, 
            List<IFormFile> eventImages, 
            IWebHostEnvironment hostEnvironment, 
            CancellationToken cancellationToken)
        {
            newEvent.EventImages = _imageService.SaveImages(eventImages, hostEnvironment);

            await _eventRepository.AddEventAsync(newEvent, cancellationToken);

            await _eventRepository.SaveChangesAsync(cancellationToken);

            _imageService.DeleteUnusedImages(hostEnvironment, _eventRepository, cancellationToken);

            return newEvent;
        }
    }

}
