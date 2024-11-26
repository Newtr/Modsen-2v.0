using System.Net;
using System.Text.Json;

namespace Modsen.API
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandleCustomResponseAsync(context, "Resource not found.");
                }
                else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    await HandleCustomResponseAsync(context, "Unauthorized access. Please log in.");
                }
                else if (context.Response.StatusCode == StatusCodes.Status409Conflict)
                {
                    await HandleCustomResponseAsync(context, "Conflict occurred while processing the request.");
                }
                else if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
                {
                    await HandleCustomResponseAsync(context, "Bad request. Please check your input.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleCustomResponseAsync(HttpContext context, string message)
        {
            context.Response.ContentType = "application/json";
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            };
            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error. Please try again later.",
                Detailed = exception.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
