using Tapawingo_backend.Models; // Importing the Models namespace

namespace Tapawingo_backend.Repository
{
    // Interface for interacting with team data
    public interface ITeamRepository
    {
        // Add a new team to the repository
        Task<Team> AddTeam(Team team);
    }
}
