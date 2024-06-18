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

        [HttpGet("teams/{teamId}/locationlogs")]
        public async Task<IActionResult> GetLocationlogsOnTeam(int teamId)
        {
            return await _locationlogsService.GetLocationlogsOnTeamAsync(teamId);
        }

        [HttpPost("teams/{teamId}/locationlogs")]
        [ProducesResponseType(200, Type = typeof(Locationlog))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateLocationlog(int teamId, [FromBody] CreateLocationlogDto model)
        {
            var response = await _locationlogsService.CreateLocationlogOnTeamAsync(teamId, model);
            return Ok(response);
        }
    }
}