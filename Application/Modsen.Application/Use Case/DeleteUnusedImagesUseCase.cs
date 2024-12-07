using Microsoft.AspNetCore.Hosting;
using Modsen.Domain;
using Modsen.Infrastructure;

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