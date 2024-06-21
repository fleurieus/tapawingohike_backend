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

        [HttpGet("organisations/{organisationId}/Events")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EventDto>))]
        [ProducesResponseType(404)]
        public IActionResult GetEventsByOrganisationId(int organisationId)
        {
            return _eventsService.GetEventsByOrganisationId(organisationId);
        }
        
        [HttpGet("organisations/{organisationId}/Events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(EventDto))]
        [ProducesResponseType(404)]
        public IActionResult GetEventById(int eventId, int organisationId)
        {
            return _eventsService.GetEventByIdAndOrganisationId(eventId, organisationId);
        }

        [HttpPost("organisations/{organisationId}/Events")]
        [ProducesResponseType(200, Type = typeof(Event))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        public IActionResult CreateEvent([FromBody] CreateEventDto model, int organisationId)
        {
            var response = _eventsService.CreateEvent(model, organisationId);
            return response;
        }
        
        [HttpPut("organisations/{organisationId}/Events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(Event))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(404)]
        public IActionResult UpdateEvent([FromBody] CreateEventDto model, int eventId, int organisationId)
        {
            var response = _eventsService.UpdateEvent(model, organisationId, eventId);
            return response;
        }
        
        [HttpDelete("organisations/{organisationId}/events/{eventId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteEvent(int eventId, int organisationId)
        {
            return _eventsService.DeleteEvent(eventId, organisationId);
        }
    }
}
