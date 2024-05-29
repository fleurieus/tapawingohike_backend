using Microsoft.AspNetCore.Identity;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(DataContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public User GetUser(string guid)
        {
            return _context.Users.Where(u => u.Id == guid).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.Where(u => u.Email == email).FirstOrDefault();
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public bool UserExists(string userGuid)
        {
            bool userExists = _context.Users.Any(u => u.Id == userGuid);
            return userExists;
        }

        public async Task<bool> TryUpdateUser(User existingUser, UpdateUserDto user)
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
            return true;
        }

        public bool DeleteUser(string userGuid)
        {
            if (userGuid == null) return false;
            if (!UserExists(userGuid)) return false;

            try
            {
                _context.Users.Remove(GetUser(userGuid));
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
