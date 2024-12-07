using Microsoft.AspNetCore.Hosting;
using Modsen.Domain;

namespace Modsen.Application
{
    public class DeleteEventUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly ImageService _imageService;

        public DeleteEventUseCase(IEventRepository eventRepository, ImageService imageService)
        {
            _eventRepository = eventRepository;
            _imageService = imageService;
        }

        public async Task DeleteEventAsync(int eventId, IWebHostEnvironment hostEnvironment, CancellationToken cancellationToken)
        {
            var myEvent = await _eventRepository.GetEventByIdAsync(eventId, cancellationToken);

            if (myEvent == null)
                throw new NotFoundException("Event not found.");

            _imageService.DeleteImages(myEvent.EventImages, hostEnvironment);

            await _eventRepository.DeleteEventAsync(myEvent, cancellationToken);
        }
    }

}
