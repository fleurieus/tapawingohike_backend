using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
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
        [ProducesResponseType(200, Type = typeof(Team))] // HTTP 200: Successful creation, returns the created team
        [ProducesResponseType(400)] // HTTP 400: Bad request, if the request body is invalid
        [ProducesResponseType(500)] // HTTP 500: Internal server error, if an unexpected exception occurs
        public async Task<IActionResult> CreateTeam(CreateTeamDto model)
        {
            try
            {
                // Attempt to create the team using the provided model
                var team = await _teamService.CreateTeam(model);

                // If the team is successfully created
                if (team != null)
                {
                    // Return HTTP 200 OK status with the created team in the response body
                    return Ok(team);
                }
                else
                {
                    // If the team is null, return HTTP 400 Bad Request status
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                // Log the exception

                // Return HTTP 500 Internal Server Error status with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating team");
            }
        }

    }
}
