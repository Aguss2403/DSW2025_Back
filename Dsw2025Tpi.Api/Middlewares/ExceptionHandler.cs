using Dsw2025Tpi.Application.Exceptions;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;

namespace Dsw2025Tpi.Api.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception occurred: {ExceptionType}", e.GetType().Name);
                await HandleExceptionAsync(context, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";

            var statusCode = e switch
            {
                    Dsw2025Tpi.Application.Exceptions.ApplicationException _ => HttpStatusCode.BadRequest,
                InvalidCredentialsException _ => HttpStatusCode.BadRequest,
                InvalidCredentialException _ => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            var message = e switch
            {
                Dsw2025Tpi.Application.Exceptions.ApplicationException _ => e.Message,
                InvalidCredentialsException _ => "Invalid username or password.",
                InvalidCredentialException _ => "Unauthorized access.",
                _ => "Internal server error."
            };

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                Detail = e.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}