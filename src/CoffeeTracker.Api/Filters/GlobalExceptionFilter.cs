using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace CoffeeTracker.Api.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is JsonException)
            {
                _logger.LogWarning("Invalid JSON in request: {Exception}", context.Exception.Message);

                var problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Bad Request",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Invalid JSON in request body.",
                    Instance = context.HttpContext.Request.Path
                };

                var result = new BadRequestObjectResult(problemDetails);
                result.ContentTypes.Add("application/problem+json");
                context.Result = result;
                context.ExceptionHandled = true;
            }
            else if (context.Exception is InvalidOperationException && 
                     context.Exception.Message.Contains("JSON"))
            {
                _logger.LogWarning("JSON parsing error: {Exception}", context.Exception.Message);

                var problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Bad Request",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Invalid JSON in request body.",
                    Instance = context.HttpContext.Request.Path
                };

                var result = new BadRequestObjectResult(problemDetails);
                result.ContentTypes.Add("application/problem+json");
                context.Result = result;
                context.ExceptionHandled = true;
            }
        }
    }
}
