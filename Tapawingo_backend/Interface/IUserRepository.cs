using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IUserRepository
    {
        ICollection<User> GetUsersOnOrganisation(int organisationId);
        Task<User> GetUserOnOrganisationAsync(int organisationId, string userId);
        User GetUserByEmail(string email);
        bool UserExists(string userGuid);
        Task<User> CreateUserOnOrganisation(int organisationId, CreateUserDto model);
        Task<User> UpdateUserOnOrganisationAsync(User existingUser, UpdateUserDto user);
        Task<bool> DeleteUserOnOrganisationAsync(int organisationId, string userGuid);
    }
}
