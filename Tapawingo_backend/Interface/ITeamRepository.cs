using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public interface ITeamRepository
    {
        Task<Team> CreateTeamOnEdition(int editionId, CreateTeamDto team);
    }
}
