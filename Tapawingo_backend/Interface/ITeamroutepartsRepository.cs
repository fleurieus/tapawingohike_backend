using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface ITeamroutepartsRepository
    {
        Task<TeamRoutepart> UpdateTeamRoutePart(int teamId, int routepartId, bool finished);
    }
}
