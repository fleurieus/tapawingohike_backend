using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUOrEventUserPolicy")]
        [HttpGet("/editions/{editionId}/teams")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeamsOnEdition(int editionId)
        {
            try
            {
                var response =  await _teamService.GetTeamsOnEdition(editionId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUOrEventUserPolicy")]
        [HttpGet("/editions/{editionId}/teams/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeamOnEditionAsync(int editionId, int teamId)
        {
            try
            {
                var response = await _teamService.GetTeamOnEditionAsync(editionId, teamId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(new
                {
                    message = ex.Message
                });
            }
            
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUOrEventUserPolicy")]
        [HttpPost("/editions/{editionId}/teams")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateTeamOnEdition(int editionId, CreateTeamDto model)
        {
            try
            {
                var response = await _teamService.CreateTeamOnEditionAsync(editionId, model);
                return Ok(response);
            }
            catch (BadHttpRequestException ex)
            {
                return new NotFoundObjectResult(new
                {
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return new BadRequestObjectResult(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUOrEventUserPolicy")]
        [HttpPatch("/editions/{editionId}/teams/{teamId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTeamOnEdition(int editionId, int teamId, [FromBody] UpdateTeamDto model)
        {
            try
            {
                var response = await _teamService.UpdateTeamOnEditionAsync(editionId, teamId, model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUOrEventUserPolicy")]
        [HttpDelete("/editions/{editionId}/teams/{teamId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteTeamOnEditionAsync(int editionId, int teamId)
        {
            return await _teamService.DeleteTeamOnEditionAsync(editionId, teamId);
        }

        [HttpGet("/teams/{teamCode}")]
        public async Task<IActionResult> LoginWithTeamCode(string teamCode)
        {
            return await _teamService.LoginWithTeamCode(teamCode);
        }
    }
}
