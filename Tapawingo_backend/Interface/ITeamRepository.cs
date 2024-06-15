using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public interface ITeamRepository
    {
        ICollection<Team> GetTeamsOnEdition(int editionId);
        Task<Team> GetTeamOnEditionAsync(int editionId, int teamId);
        bool TeamExists(int teamId);
        Task<Team> CreateTeamOnEditionAsync(int editionId, CreateTeamDto team);
    }
}
