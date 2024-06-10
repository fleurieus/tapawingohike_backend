using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Helper
{
    public static class AdminInitializer
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole> _roleManager, UserManager<User> _userManager)
        {

            var admin = new User
            {
                FirstName = "super",
                LastName = "admin",
                UserName = "superadmin@example.com",
                Email = "superadmin@example.com",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            await _userManager.CreateAsync(admin, "Password!1");

            var claim = new Claim("SuperAdminRole", "SuperAdmin");

            await _userManager.AddClaimAsync(admin, claim);
        }
    }
}
