using Microsoft.AspNetCore.Hosting;
using Modsen.Domain;

namespace Modsen.Application
{
public class DeleteUnusedImagesUseCase
{
    public void Execute(IWebHostEnvironment hostEnvironment, IEventRepository eventRepository, CancellationToken cancellationToken)
    {
        MyHelpers.DeleteUnusedImagesAsync(hostEnvironment, eventRepository, cancellationToken);
    }
}
}