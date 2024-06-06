using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IUserRepository
    {
        ICollection<User> GetUsersOnOrganisation(int organisationId);
        User GetUserOnOrganisation(int organisationId, string userId);
        User GetUserByEmailAsync(string email);
        bool UserExists(string userGuid);
        Task<User> CreateUserOnOrganisation(int organisationId, CreateUserDto model);
        Task<User> UpdateUserOnOrganisationAsync(User existingUser, UpdateUserDto user);
        bool DeleteUserOnOrganisation(int organisationId, string userGuid);
    }
}
