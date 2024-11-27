using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Modsen.Domain;

namespace Modsen.Application
{
public class SaveImagesUseCase
{
    public List<EventImage> Execute(List<IFormFile> images, IWebHostEnvironment hostEnvironment)
    {
        var savedImages = new List<EventImage>();
        foreach (var image in images)
        {
            string imagePath = MyHelpers.SaveImage(image, hostEnvironment);
            savedImages.Add(new EventImage { ImagePath = imagePath });
        }
        return savedImages;
    }
}
}