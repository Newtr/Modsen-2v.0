using System.Net;
using Microsoft.AspNetCore.Http;
using Modsen.Domain;
using Newtonsoft.Json;

namespace Modsen.API
{
    public class ExceptionHandlerService : IExceptionHandler
    {
        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred.";

            switch (exception)
            {
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;
                case BadRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;
                case UnauthorizedException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = exception.Message;
                    break;
            }

            var response = new { error = message };
            var payload = JsonConvert.SerializeObject(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(payload);
        }
    }
}
