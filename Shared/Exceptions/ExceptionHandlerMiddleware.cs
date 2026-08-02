using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Shared.Exceptions
{
    public class ExeptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ExeptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
                BadEnvironmentValue => HttpStatusCode.InternalServerError,
                BadDataException => HttpStatusCode.BadRequest,
                NotFoundExeption => HttpStatusCode.NotFound,
                UnauthorizedException => HttpStatusCode.Unauthorized,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError,
            };

            var response = new
            {
                statusCode = (int)statusCode,
                message = exception.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
