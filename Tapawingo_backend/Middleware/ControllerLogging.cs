using Microsoft.AspNetCore.Mvc.Controllers;

namespace Tapawingo_backend.Middleware
{
    public class ControllerLogging
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ControllerLogging> _logger;

        public ControllerLogging(RequestDelegate next, ILogger<ControllerLogging> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                if (actionDescriptor != null)
                {
                    // Log controller action
                    var controllerAction = $"{actionDescriptor.ControllerName}.{actionDescriptor.ActionName}";
                    _logger.LogInformation($"Controller action executed: {controllerAction}");
                }
            }

            await _next(context);
        }
    }
}
