using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace Tapawingo_backend.Middleware
{
    public class ServiceLogging
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ServiceLogging> _logger;

        public ServiceLogging(RequestDelegate next, ILogger<ServiceLogging> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var method = GetServiceMethod(endpoint, serviceProvider);
                if (method != null)
                {
                    // Log service action
                    var action = $"{method.DeclaringType.Name}.{method.Name}";
                    _logger.LogInformation($"Service action executed: {action}");
                }
            }

            await _next(context);
        }

        private MethodInfo GetServiceMethod(Endpoint endpoint, IServiceProvider serviceProvider)
        {
            var methodMetadata = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (methodMetadata != null)
            {
                var controllerType = methodMetadata.ControllerTypeInfo.AsType();
                var serviceTypes = controllerType.Assembly.GetTypes()
                    .Where(t => !t.IsInterface);

                foreach (var serviceType in serviceTypes)
                {
                    var serviceInstance = serviceProvider.GetService(serviceType);
                    var serviceMethod = serviceType.GetMethod(methodMetadata.ActionName);
                    if (serviceMethod != null)
                        return serviceMethod;
                }
            }

            return null;
        }
    }
}
