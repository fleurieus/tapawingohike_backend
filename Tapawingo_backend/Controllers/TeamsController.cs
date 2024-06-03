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
                /** /Check to see that the Edition object for the EditionId passed in the CreateTeamDto exists
                // Try to find the edition
                var edition = await _editionService.GetEditionById(model.EditionId);
                if (edition == null) {
                return StatusCode(StatusCodes.Status400BadRequest, "The edition for the team does not exist");
                //Add logging for this maybe.
                }**/
                
                // Attempt to create the team using the provided model
                var team = await _teamService.CreateTeam(model);
                return team != null ? Ok(team) : BadRequest("Error creating team");
            }
            catch (Exception)
            {
                // Log the exception
                // Return HTTP 500 Internal Server Error status with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating team");
            }
        }
    }
}
