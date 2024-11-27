using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Modsen.Domain;
using Modsen.Infrastructure;

namespace Modsen.Application
{
    public class UpdateEventUseCase
    {
        private readonly ModsenContext _context;
        private readonly ImageService _imageService;
        private readonly EmailService _emailService;

        public UpdateEventUseCase(ModsenContext context, ImageService imageService, EmailService emailService)
        {
            _context = context;
            _imageService = imageService;
            _emailService = emailService;
        }

        public async Task UpdateEventAsync(int eventId, MyEvent updatedEvent, List<IFormFile> eventImages, string userEmail, IWebHostEnvironment hostEnvironment, CancellationToken cancellationToken)
        {
            var existingEvent = await _context.Events
                .Include(e => e.EventImages)
                .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

            if (existingEvent == null)
                throw new NotFoundException("Event not found.");

            existingEvent.Name = updatedEvent.Name;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.DateOfEvent = updatedEvent.DateOfEvent;
            existingEvent.EventLocation = updatedEvent.EventLocation;
            existingEvent.EventCategory = updatedEvent.EventCategory;
            existingEvent.MaxMember = updatedEvent.MaxMember;

            _context.EventImages.RemoveRange(existingEvent.EventImages);
            existingEvent.EventImages = _imageService.SaveImages(eventImages, hostEnvironment);

            await _context.SaveChangesAsync(cancellationToken);

            _emailService.SendEventUpdatedEmail(userEmail, eventId);

            _imageService.DeleteUnusedImages(hostEnvironment, _context);
        }
    }
}
