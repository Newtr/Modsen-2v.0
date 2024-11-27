using Microsoft.AspNetCore.Http;
using Modsen.Domain;
using System;
using System.Threading.Tasks;

namespace Modsen.API
{
    public class CheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionHandler _exceptionHandler;

        public CheckMiddleware(RequestDelegate next, IExceptionHandler exceptionHandler)
        {
            _next = next;
            _exceptionHandler = exceptionHandler;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await _exceptionHandler.HandleExceptionAsync(context, ex);
            }
        }
    }
}
