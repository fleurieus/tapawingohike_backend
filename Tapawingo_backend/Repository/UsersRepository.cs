using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<User> GetUserOnOrganisationAsync(int organisationId, string userId)
        {
            return await _context.UserOrganisations
                                 .Where(uo => uo.OrganisationId == organisationId && uo.UserId == userId)
                                 .Select(user => user.User)
                                 .FirstOrDefaultAsync();
        }

        public User GetUserByEmail(string email)
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

            return newUser;
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

        public async Task<bool> DeleteUserOnOrganisationAsync(int organisationId, string userGuid)
        {
            try
            {
                _context.Users.Remove(await GetUserOnOrganisationAsync(organisationId, userGuid));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
