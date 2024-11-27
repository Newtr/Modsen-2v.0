using Microsoft.AspNetCore.Hosting;
using Modsen.Infrastructure;

namespace Modsen.Application
{
public class DeleteUnusedImagesUseCase
{
    public void Execute(IWebHostEnvironment hostEnvironment, ModsenContext context)
    {
        MyHelpers.DeleteUnusedImages(hostEnvironment, context);
    }
}
}