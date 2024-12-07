using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Modsen.Domain;
using Modsen.Infrastructure;

namespace Modsen.Application
{
    public class UpdateEventUseCase
{
    private readonly IEventRepository _eventRepository;
    private readonly ImageService _imageService;
    private readonly EmailService _emailService;
    private readonly IMapper _mapper;

    public UpdateEventUseCase(
        IEventRepository eventRepository,
        ImageService imageService,
        EmailService emailService,
        IMapper mapper)
    {
        _eventRepository = eventRepository;
        _imageService = imageService;
        _emailService = emailService;
        _mapper = mapper;
    }

    public async Task UpdateEventAsync(
        int eventId,
        MyEvent updatedEvent,
        List<IFormFile> eventImages,
        string userEmail,
        IWebHostEnvironment hostEnvironment,
        CancellationToken cancellationToken)
    {
        var existingEvent = await _eventRepository.GetEventWithImagesAsync(eventId, cancellationToken);

        if (existingEvent == null)
            throw new NotFoundException("Event not found.");

        _mapper.Map(updatedEvent, existingEvent);

        existingEvent.EventImages = _imageService.SaveImages(eventImages, hostEnvironment);

        await _eventRepository.UpdateAsync(existingEvent, cancellationToken);

        _emailService.SendEventUpdatedEmail(userEmail, eventId);

        _imageService.DeleteUnusedImages(hostEnvironment, _eventRepository, cancellationToken);
    }
}

}
