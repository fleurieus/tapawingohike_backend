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
        [ProducesResponseType(201, Type = typeof(Team))] // HTTP 200: Successful creation, returns the created team
        [ProducesResponseType(400)] // Bad request, if the request body is invalid
        [ProducesResponseType(401)] // The user is logged in and therefore not authenticated.
        [ProducesResponseType(403)] // The user is not authorized to create a team
        [ProducesResponseType(409)] // Conflict - the team that is attempting to be created already exists
        [ProducesResponseType(500)] // Internal server error, if an unexpected exception occurs
        
        public IActionResult CreateTeam(CreateTeamDto model)
        {

            try
            {
                /** //Check to see that the Edition object for the EditionId passed in the CreateTeamDto exists
                // Try to find the edition
                var edition = await _editionService.GetEditionById(model.EditionId);
                if (edition == null) {
                return StatusCode(StatusCodes.Status400BadRequest, "The edition for the team does not exist");
                //Add logging for this maybe.
                } **/   
                
                // Attempt to create the team using the provided model
                return new ObjectResult(_teamService.CreateTeam(model))
                {
                    StatusCode = 201
                };
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
