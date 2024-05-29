using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
using Tapawingo_backend.Interface;

namespace Tapawingo_backend.Middleware
{
    public class RepositoryLogging
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RepositoryLogging> _logger;

        public RepositoryLogging(RequestDelegate next, ILogger<RepositoryLogging> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var method = GetRepositoryMethod(endpoint, serviceProvider);
                if (method != null)
                {
                    // Log repository action
                    var action = $"{method.DeclaringType.Name}.{method.Name}";
                    _logger.LogInformation($"Repository action executed: {action}");
                }
            }

            await _next(context);
        }

        private MethodInfo GetRepositoryMethod(Endpoint endpoint, IServiceProvider serviceProvider)
        {
            var methodMetadata = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (methodMetadata != null)
            {
                var controllerType = methodMetadata.ControllerTypeInfo.AsType();
                var repositoryInterfaceType = typeof(IUserRepository);
                var repositoryTypes = controllerType.Assembly.GetTypes()
                    .Where(t => repositoryInterfaceType.IsAssignableFrom(t) && !t.IsInterface);

                foreach (var repositoryType in repositoryTypes)
                {
                    var repositoryInstance = serviceProvider.GetService(repositoryType);
                    var repositoryMethod = repositoryType.GetMethod(methodMetadata.ActionName);
                    if (repositoryMethod != null)
                        return repositoryMethod;
                }
            }

            return null;
        }
    }
}
