using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly TeamService _teamService;

        public TeamsController(TeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam(CreateTeamDto model)
       {
    try
    {
        var team = await _teamService.CreateTeam(model);
        if (team != null)
        {
            return Ok(team); // Return the created team
        }
        else
        {
            return BadRequest(); // Or return a boolean indicating failure
        }
    }
    catch (Exception ex)
    {
        // Log the exception
        return StatusCode(StatusCodes.Status500InternalServerError, "Error creating team");
    }
}

        // Other actions...
    }
}
