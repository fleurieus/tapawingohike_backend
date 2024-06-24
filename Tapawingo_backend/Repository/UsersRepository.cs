using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;
using System.Security.Claims;
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

        public UsersRepository(DataContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<ICollection<User>> GetUsersOnOrganisation(int organisationId)
        {
            return await _context.UserOrganisations.Where(uo => uo.OrganisationId == organisationId).Select(user => user.User).OrderBy(u => u.FirstName).ToListAsync();
        }

        public async Task<User> GetUserOnOrganisationAsync(int organisationId, string userId)
        {
            return await _context.UserOrganisations
                                 .Where(uo => uo.OrganisationId == organisationId && uo.UserId == userId)
                                 .Select(user => user.User)
                                 .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UserExists(string userGuid)
        {
            bool userExists = await _context.Users.AnyAsync(u => u.Id == userGuid);
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
                var errors = result.Errors.Select(e => e.Description);
                var errorMessage = string.Join(", ", errors);
                throw new Exception($"message: {errorMessage}");
            }

            // Add user to organisation
            _context.UserOrganisations.Add(new UserOrganisation { OrganisationId = organisationId, UserId = newUser.Id });
            await _context.SaveChangesAsync();

            // Add claim that gives user acces to organisation
            var userClaim = new Claim("Claim", $"{organisationId}:OrganisationUser");

            if (model.IsManager)
            {
                userClaim = new Claim("Claim", $"{organisationId}:OrganisationManager");
            }

            var claimResult = await _userManager.AddClaimAsync(newUser, userClaim);
            if (!claimResult.Succeeded)
            {

                var errors = result.Errors.Select(e => e.Description);
                var errorMessage = string.Join(", ", errors);
                throw new Exception($"message: {errorMessage}");
            }

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

        // User on event
        public async Task<ICollection<User>> GetUsersOnEvent(int eventId)
        {
            return await _context.UserEvents.Where(uo => uo.EventId == eventId).Select(user => user.User).OrderBy(u => u.FirstName).ToListAsync();
        }

        public async Task<User> GetUserOnEventAsync(int eventId, string userId)
        {
            return await _context.UserEvents
                .Where(uo => uo.EventId == eventId && uo.UserId == userId)
                .Select(user => user.User)
                .FirstOrDefaultAsync();
        }

        public async Task<User> CreateUserOnEvent(int eventId, CreateUserOnEventDto model)
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

            // Add user to event
            _context.UserEvents.Add(new UserEvent { EventId = eventId, UserId = newUser.Id });
            await _context.SaveChangesAsync();

            // Add claim that gives user acces to event
            var userClaim = new Claim("Claim", $"{eventId}:EventUser");


            var claimResult = await _userManager.AddClaimAsync(newUser, userClaim);
            if (!claimResult.Succeeded)
            {
                throw new Exception("Something went worng adding claim to user.");
            }

            return newUser;
        }

        public async Task<User> UpdateUserOnEventAsync(User existingUser, UpdateUserOnEventDto user)
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

        public async Task<bool> DeleteUserOnEventAsync(int eventId, string userGuid)
        {
            try
            {
                _context.Users.Remove(await GetUserOnEventAsync(eventId, userGuid));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UserExistsOnOrganisation(string userId, int organisationId)
        {
            var result = await _context.UserOrganisations.FirstOrDefaultAsync(uo => uo.UserId.Equals(userId) && uo.OrganisationId == organisationId);
            return result != null;
        }

        public async Task<bool> UserExistsOnEvent(string userId, int eventId)
        {
            var result = await _context.UserEvents.FirstOrDefaultAsync(ue => ue.UserId.Equals(userId) && ue.EventId == eventId);
            return result != null;
        }
    }
}
