using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Tapawingo_backend
{
    public class GlobalErrorHandling(RequestDelegate next, ILogger<GlobalErrorHandling> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalErrorHandling> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            switch (exception)
            {
                case ValidationException validationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new { Errors = validationException.InnerException });
                    _logger.LogError(validationException.Message);
                    break;

                case UnauthorizedAccessException unauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { Message = unauthorizedAccessException.Message ?? "Unauthorized" });
                    _logger.LogError(unauthorizedAccessException.Message);
                    break;

                case BadHttpRequestException badHttpRequestException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new { Message = badHttpRequestException.Message ?? "Bad Request" });
                    _logger.LogError(badHttpRequestException.Message);
                    break;

                case DetailedIdentityErrorException detailedIdentityErrorException:
                    context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    await context.Response.WriteAsJsonAsync(new { Message = detailedIdentityErrorException.Message });
                    _logger.LogError(detailedIdentityErrorException.Message);
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsJsonAsync(new { Message = "Internal Server Error." });
                    _logger.LogError(exception.Message);
                    break;
            }
        }
    }
}
