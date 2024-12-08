using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Modsen.Domain;

namespace Modsen.Application
{
public class SaveImagesUseCase
{
    public List<EventImage> Execute(List<IFormFile> eventImages, IWebHostEnvironment hostEnvironment)
    {
        var uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "Images");

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var savedImages = new List<EventImage>();

        foreach (var eventImage in eventImages)
        {
            string uniqueFileName;
            string filePath;

            do
            {
                int randomNumber = new Random().Next(1, 1_000_000);
                uniqueFileName = $"Imo{randomNumber}.jpg";
                filePath = Path.Combine(uploadsFolder, uniqueFileName);

            } while (File.Exists(filePath));

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                eventImage.CopyTo(fileStream); 
            }

            var savedImage = new EventImage
            {
                ImagePath = Path.Combine("Images", uniqueFileName) 
            };

            savedImages.Add(savedImage); 
        }

        return savedImages; 
    }
}

}