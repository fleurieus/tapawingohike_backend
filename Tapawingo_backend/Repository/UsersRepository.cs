using Microsoft.AspNetCore.Identity;
using Serilog;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public class UsersRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersRepository(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public ICollection<User> GetUsersOnOrganisation(int organisationId)
        {
            return _context.UserOrganisations.Where(uo => uo.OrganisationId == organisationId).Select(user => user.User).OrderBy(u => u.FirstName).ToList();
        }

        public User GetUserOnOrganisation(int organisationId, string userId)
        {
            return _context.UserOrganisations.Where(uo => uo.OrganisationId == organisationId && uo.UserId == userId).Select(user => user.User).FirstOrDefault();
        }

        public User GetUserByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public bool UserExists(string userGuid)
        {
            bool userExists = _context.Users.Any(u => u.Id == userGuid);
            return userExists;
        }

        public async Task<User> CreateUserOnOrganisation(int organisationId, CreateUserDto model)
        {
            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                throw new Exception("User creation failed.");
            }

            _context.UserOrganisations.Add(new UserOrganisation { OrganisationId = organisationId, UserId = newUser.Id });
            await _context.SaveChangesAsync();

            //Assign Default Role : Admin to first registrar; rest is user
            var checkAdmin = _roleManager.FindByNameAsync("SuperAdmin");
            if (checkAdmin is null)
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = "SuperAdmin" });
                await _userManager.AddToRoleAsync(newUser, "SuperAdmin");
                return newUser;
            }
            else
            {
                var checkUser = _roleManager.FindByNameAsync("User");
                if (checkUser is null)
                    await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });

                await _userManager.AddToRoleAsync(newUser, "User");
                return newUser;
            }
        }

        public async Task<User> UpdateUserOnOrganisationAsync(User existingUser, UpdateUserDto user)
        {
            if (user.CurrentPassword != null && user.NewPassword != null)
            {
                await _userManager.ChangePasswordAsync(existingUser, user.CurrentPassword, user.NewPassword);
            }

            if (user.Email != null) await _userManager.ChangeEmailAsync(existingUser, user.Email, await _userManager.GenerateChangeEmailTokenAsync(existingUser, user.Email));
            if (user.FirstName != null)
            {
                existingUser.FirstName = user.FirstName;
                _context.Users.Update(existingUser);
                _context.SaveChanges();
            }
            if (user.LastName != null)
            {
                existingUser.LastName = user.LastName;
                _context.Users.Update(existingUser);
                _context.SaveChanges();
            }

            return existingUser;
        }

        public bool DeleteUserOnOrganisation(int organisationId, string userGuid)
        {
            try
            {
                _context.Users.Remove(GetUserOnOrganisation(organisationId, userGuid));
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
