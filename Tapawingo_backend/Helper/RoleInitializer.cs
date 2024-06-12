using Microsoft.AspNetCore.Identity;

namespace Tapawingo_backend.Helper
{
    public static class RoleInitializer
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "SuperAdmin", "OrganizationManager", "EventUser", "OrganizationUser" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
