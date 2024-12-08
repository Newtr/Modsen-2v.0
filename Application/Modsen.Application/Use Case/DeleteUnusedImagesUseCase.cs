using Microsoft.AspNetCore.Hosting;
using Modsen.Domain;

namespace Modsen.Application
{
public class DeleteUnusedImagesUseCase
{
    public async Task Execute(IWebHostEnvironment hostEnvironment, IEventRepository eventRepository, CancellationToken cancellationToken)
    {
        var uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "Images");

        var allFiles = Directory.GetFiles(uploadsFolder);
        var usedImages = await eventRepository.GetAllEventImagePathsAsync(cancellationToken);

        foreach (var filePath in allFiles)
        {
            var fileName = Path.GetFileName(filePath);
            if (!usedImages.Contains("Images\\" + fileName))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
}