using Microsoft.AspNetCore.Hosting;
using Modsen.Domain;

namespace Modsen.Application
{
public class DeleteImagesUseCase
{
    public void Execute(IEnumerable<EventImage> eventImages, IWebHostEnvironment hostEnvironment)
    {
        foreach (var eventImage in eventImages)
        {
            var imagePath = Path.Combine(hostEnvironment.WebRootPath, eventImage.ImagePath);
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
    }
}
}