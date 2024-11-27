
using Microsoft.AspNetCore.Http;

namespace Modsen.Domain
{
    public interface IExceptionHandler
    {
        Task HandleExceptionAsync(HttpContext context, Exception exception);
    }
}
