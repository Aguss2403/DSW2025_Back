using Dsw2025Tpi.Application.Exceptions;
using System.Net;
using System.Security.Authentication; 
using System.Text.Json;
using ApplicationException = Dsw2025Tpi.Application.Exceptions.ApplicationException;

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
                _logger.LogError(e, "Ocurrió una excepción: {ExceptionType}", e.GetType().Name);
                await HandleExceptionAsync(context, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";

            var statusCode = e switch
            {
                ValidationException => HttpStatusCode.BadRequest,
                InvalidFormatSKUException => HttpStatusCode.BadRequest,
                InvalidStatusException => HttpStatusCode.BadRequest,

                DuplicatedEntityException => HttpStatusCode.Conflict,

                InvalidCredentialsException => HttpStatusCode.Unauthorized,
                NotAuthenticateException => HttpStatusCode.Unauthorized,

                NotFoundException => HttpStatusCode.NotFound,
                EntityNotFoundException => HttpStatusCode.NotFound,

                NoContentException => HttpStatusCode.NoContent,

                ApplicationException => HttpStatusCode.BadRequest,

                KeyNotFoundException => HttpStatusCode.NotFound,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,

                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            if (statusCode == HttpStatusCode.NoContent)
            {
                return Task.CompletedTask;
            }

            int responseCode = 0;
            string responseMessage = "Ha ocurrido un error interno en el servidor.";
            string responseDetail = e.Message; 

            if (e is ApplicationException appEx)
            {
                responseCode = appEx.Code;      
                responseMessage = appEx.Message; 
            }

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = responseMessage, 
                code = responseCode,       
                detail = responseDetail    
            };

            var jsonResponse = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}