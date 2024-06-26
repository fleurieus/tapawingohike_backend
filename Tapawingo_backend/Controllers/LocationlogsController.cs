using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Models;
using Tapawingo_backend.Services;
using System.Linq;
using Tapawingo_backend.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Tapawingo_backend.Controllers
{
    [ApiController]
    public class LocationlogsController : ControllerBase
    {
        private readonly LocationlogsService _locationlogsService;

        public LocationlogsController(LocationlogsService locationlogsService)
        {
            _locationlogsService = locationlogsService;
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUOrEventUserPolicy")]
        [HttpGet("teams/{teamId}/locationlogs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLocationlogsOnTeam(int teamId)
        {
            return await _locationlogsService.GetLocationlogsOnTeamAsync(teamId);
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUOrEventUserPolicy")]
        [HttpPost("teams/{teamId}/locationlogs")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Locationlog))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateLocationlog(int teamId, [FromBody] CreateLocationlogDto model)
        {
            try
            {
                var response = await _locationlogsService.CreateLocationlogOnTeamAsync(teamId, model);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return new NotFoundObjectResult(new
                {
                    message = ex.Message
                });
            }
            
        }
    }
}