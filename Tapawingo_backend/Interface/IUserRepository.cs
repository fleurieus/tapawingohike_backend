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
        Task<bool> UserExistsOnOrganisation(string userId, int organisationId);

        // User on events
        ICollection<User> GetUsersOnEvent(int eventId);
        Task<User> GetUserOnEventAsync(int eventId, string userId);
        Task<User> CreateUserOnEvent(int eventId, CreateUserOnEventDto model);
        Task<User> UpdateUserOnEventAsync(User existingUser, UpdateUserOnEventDto user);
        Task<bool> DeleteUserOnEventAsync(int eventId, string userGuid);
        Task<bool> UserExistsOnEvent(string userId, int eventId);

    }
}
