using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Controllers
{
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly TeamService _teamService;

        public TeamsController(TeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost("/editions/{editionId}/teams")]
        public IActionResult CreateTeamOnEdition(int editionId, CreateTeamDto model)
        {
            var response = _teamService.CreateTeamOnEdition(editionId, model);
            return Ok(response);
        }
    }
}
