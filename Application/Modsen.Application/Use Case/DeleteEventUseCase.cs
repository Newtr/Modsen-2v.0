using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Modsen.Domain;
using Modsen.Infrastructure;

namespace Modsen.Application
{
public class DeleteEventUseCase
{
    private readonly ModsenContext _context;
    private readonly ImageService _imageService;

    public DeleteEventUseCase(ModsenContext context, ImageService imageService)
    {
        _context = context;
        _imageService = imageService;
    }

    public async Task DeleteEventAsync(int eventId, IWebHostEnvironment hostEnvironment)
    {
        var myEvent = await _context.Events.Include(e => e.EventImages).FirstOrDefaultAsync(e => e.Id == eventId);
        if (myEvent == null)
            throw new NotFoundException("Event not found.");

        _imageService.DeleteImages(myEvent.EventImages, hostEnvironment);
        _context.EventImages.RemoveRange(myEvent.EventImages);
        _context.Events.Remove(myEvent);

        await _context.SaveChangesAsync();
    }
}
}