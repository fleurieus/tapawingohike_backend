using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Models;
using Tapawingo_backend.Services;
using System.Linq;
using Tapawingo_backend.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Tapawingo_backend.Controllers
{
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventsService _eventsService;

        public EventsController(EventsService eventsService)
        {
            _eventsService = eventsService;
        }

        [Authorize]
        [HttpGet("organisations/{organisationId}/Events")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EventDto>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEventsByOrganisationId(int organisationId)
        {
            var userClaim = User.Claims.ToArray()[5].Value;
            return await _eventsService.GetEventsByOrganisationId(organisationId, userClaim);
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUOrEventUserPolicy")]
        [HttpGet("organisations/{organisationId}/Events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(EventDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEventById(int eventId, int organisationId)
        {
            return await _eventsService.GetEventByIdAndOrganisationId(eventId, organisationId);
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUPolicy")]
        [HttpPost("organisations/{organisationId}/Events")]
        [ProducesResponseType(200, Type = typeof(Event))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto model, int organisationId)
        {
            var response = await _eventsService.CreateEvent(model, organisationId);
            return response;
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUOrEventUserPolicy")]
        [HttpPut("organisations/{organisationId}/Events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(Event))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateEvent([FromBody] CreateEventDto model, int eventId, int organisationId)
        {
            var response = await _eventsService.UpdateEvent(model, organisationId, eventId);
            return response;
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUPolicy")]
        [HttpDelete("organisations/{organisationId}/events/{eventId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteEvent(int eventId, int organisationId)
        {
            return await _eventsService.DeleteEvent(eventId, organisationId);
        }
    }
}
