using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Modsen.Domain;
using Modsen.Infrastructure;

namespace Modsen.Application
{
    public class CreateEventUseCase
    {
        private readonly ModsenContext _context;
        private readonly ImageService _imageService;

        public CreateEventUseCase(ModsenContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<MyEvent> CreateEventAsync(MyEvent newEvent, List<IFormFile> eventImages, IWebHostEnvironment hostEnvironment, CancellationToken cancellationToken)
        {
            newEvent.EventImages = _imageService.SaveImages(eventImages, hostEnvironment);

            _context.Events.Add(newEvent);

            await _context.SaveChangesAsync(cancellationToken);

            _imageService.DeleteUnusedImages(hostEnvironment, _context);

            return newEvent;
        }
    }
}
