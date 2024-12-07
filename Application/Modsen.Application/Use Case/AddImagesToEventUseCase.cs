using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Modsen.Domain;

namespace Modsen.Application
{
    public class AddImagesToEventUseCase
{
    private readonly IEventRepository _eventRepository;
    private readonly ImageService _imageService;

    public AddImagesToEventUseCase(IEventRepository eventRepository, ImageService imageService)
    {
        _eventRepository = eventRepository;
        _imageService = imageService;
    }

    public async Task AddImagesToEventAsync(
        int eventId, 
        List<IFormFile> eventImages, 
        IWebHostEnvironment hostEnvironment, 
        CancellationToken cancellationToken)
    {
        var existingEvent = await _eventRepository.GetEventWithImagesAsync(eventId, cancellationToken);

        if (existingEvent == null)
            throw new NotFoundException("Event not found.");

        var images = _imageService.SaveImages(eventImages, hostEnvironment);

        existingEvent.EventImages.AddRange(images);

        await _eventRepository.SaveChangesAsync(cancellationToken);
    }
}
}
