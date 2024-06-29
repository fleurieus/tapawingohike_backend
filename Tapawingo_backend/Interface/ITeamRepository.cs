using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public interface ITeamRepository
    {
        Task<ICollection<Team>> GetTeamsOnEdition(int editionId);
        Task<Team> GetTeamOnEditionAsync(int editionId, int teamId);
        Task<bool> TeamExists(int teamId);
        Task<Team> CreateTeamOnEditionAsync(int editionId, CreateTeamDto team);
        Task<Team> UpdateTeamOnEditionAsync(Team existingTeam, UpdateTeamDto team);
        Task<bool> DeleteTeamOnEditionAsync(int editionId, int teamId);
        Task<bool> TeamExistsOnEdition(int teamId, int editionId);
    }
}
