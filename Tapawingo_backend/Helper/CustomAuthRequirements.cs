using Microsoft.AspNetCore.Authorization;

namespace Tapawingo_backend.Helper
{
    public class CustomAuthRequirements
    {
        public class OrganizationRequirement : IAuthorizationRequirement
        {
            public string OrganizationId { get; }

            public OrganizationRequirement(string organizationId)
            {
                OrganizationId = organizationId;
            }
        }

        public class OrganizationHandler : AuthorizationHandler<OrganizationRequirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrganizationRequirement requirement)
            {
                if (context.Resource is HttpContext httpContext)
                {
                    if (httpContext.Request.RouteValues.TryGetValue("organisationId", out var orgId))
                    {
                        var organizationClaim = context.User.FindFirst(c => c.Type == "organisationrole" && c.Value.StartsWith(orgId.ToString()));
                        if (organizationClaim != null)
                        {
                            context.Succeed(requirement);
                        }
                    }
                }

                return Task.CompletedTask;
            }
        }
    }
}
