using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Controllers
{
    [ApiController]
    public class TeamroutepartsController: ControllerBase
    {
        private readonly TeamroutepartsService _teamroutepartsService;

        public TeamroutepartsController(TeamroutepartsService teamroutepartsService)
        {
            _teamroutepartsService = teamroutepartsService;
        }

        [HttpPatch("teamrouteparts/teams/{teamId}/routeparts/{routepartId}")]
        public async Task<IActionResult> UpdateTeamroutepart(int teamId, int routepartId, UpdateTeamroutepartDto model)
        {
            return await _teamroutepartsService.UpdateTeamRoutepart(teamId, routepartId, model.IsFinished);
        }
    }
}
