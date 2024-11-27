using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Modsen.Domain;
using Modsen.Infrastructure;

namespace Modsen.Application
{
    public class AddImagesToEventUseCase
    {
        private readonly ModsenContext _context;
        private readonly ImageService _imageService;

        public AddImagesToEventUseCase(ModsenContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task AddImagesToEventAsync(int eventId, List<IFormFile> eventImages, IWebHostEnvironment hostEnvironment, CancellationToken cancellationToken)
        {
            var existingEvent = await _context.Events
                .Include(e => e.EventImages)
                .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

            if (existingEvent == null)
                throw new NotFoundException("Event not found.");

            var images = _imageService.SaveImages(eventImages, hostEnvironment);

            existingEvent.EventImages.AddRange(images);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
