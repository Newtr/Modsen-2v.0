using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Modsen.Domain;

namespace Modsen.Application
{
public class ImageService
{
    private readonly SaveImagesUseCase _saveImagesUseCase;
    private readonly DeleteUnusedImagesUseCase _deleteUnusedImagesUseCase;
    private readonly DeleteImagesUseCase _deleteImagesUseCase;

    public ImageService(
        SaveImagesUseCase saveImagesUseCase,
        DeleteUnusedImagesUseCase deleteUnusedImagesUseCase,
        DeleteImagesUseCase deleteImagesUseCase)
    {
        _saveImagesUseCase = saveImagesUseCase;
        _deleteUnusedImagesUseCase = deleteUnusedImagesUseCase;
        _deleteImagesUseCase = deleteImagesUseCase;
    }

    public List<EventImage> SaveImages(List<IFormFile> images, IWebHostEnvironment hostEnvironment)
    {
        return _saveImagesUseCase.Execute(images, hostEnvironment);
    }

    public void DeleteUnusedImages(IWebHostEnvironment hostEnvironment, IEventRepository eventRepository, CancellationToken cancellationToken)
    {
        _deleteUnusedImagesUseCase.Execute(hostEnvironment, eventRepository, cancellationToken);
    }

    public void DeleteImages(IEnumerable<EventImage> eventImages, IWebHostEnvironment hostEnvironment)
    {
        _deleteImagesUseCase.Execute(eventImages, hostEnvironment);
    }
}
}