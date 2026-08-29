using System.Net;
using System.Text.Json;

namespace ConferenceRoomBooking.API.Middleware
{

    // Централізовано обробляємо винятки, щоб API повертало узгоджені HTTP-коди та JSON-відповіді замість необроблених помилок
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
            }
            catch (Exception exception)
            {
                var statusCode = GetStatusCode(exception);

                if (statusCode == (int)HttpStatusCode.InternalServerError)
                {
                    _logger.LogError(exception, "An unhandled exception occurred.");
                }
                else
                {
                    _logger.LogWarning("Request failed with status {StatusCode}: {Message}", statusCode, exception.Message);
                }

                await HandleExceptionAsync(context, exception, statusCode);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                statusCode,
                message = statusCode == (int)HttpStatusCode.InternalServerError ? "An unexpected error occurred." : exception.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static int GetStatusCode(Exception exception)
        {
            return exception switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                InvalidOperationException => (int)HttpStatusCode.Conflict,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
