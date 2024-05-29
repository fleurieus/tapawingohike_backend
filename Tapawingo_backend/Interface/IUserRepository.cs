using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        User GetUser(string guid);
        User GetUserByEmail(string email);
        bool UserExists(string userGuid);
        Task<bool> TryUpdateUser(User existingUser, UpdateUserDto user);
        bool DeleteUser(string userGuid);
    }
}
