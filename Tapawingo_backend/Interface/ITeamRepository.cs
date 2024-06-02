using System.Threading.Tasks;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public interface ITeamRepository
    {
        Task<Team> AddTeam(Team team);
    }
}
