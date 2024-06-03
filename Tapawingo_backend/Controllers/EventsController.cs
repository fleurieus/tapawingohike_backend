using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Models;
using Tapawingo_backend.Services;
using System.Linq;
using Tapawingo_backend.Dtos;

namespace Tapawingo_backend.Controllers
{
    [ApiController]
    public class EventsController : Controller
    {
        private readonly EventsService _eventsService;

        public EventsController(EventsService eventsService)
        {
            _eventsService = eventsService;
        }

        [HttpGet("api/organisations/{organisationId}/Events")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Event>))]
        public IActionResult GetEventsByOrganisationId(int organisationId)
        {
            var events = _eventsService.GetEventsByOrganisationId(organisationId);
            return Ok(events);
        }
        
        [HttpPost("api/organisations/{organisationId}/Events")]
        public IActionResult CreateEvent([FromBody] CreateEventDto model, int organisationId)
        {
            try
            {
                var response = _eventsService.CreateOrUpdateEvent(model, organisationId, null);
                return Ok(response);
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
        
        [HttpPut("api/organisations/{organisationId}/Events/{eventId}")]
        public IActionResult UpdateEvent([FromBody] CreateEventDto model, int eventId, int organisationId)
        {
            try
            {
                var response = _eventsService.CreateOrUpdateEvent(model, organisationId, eventId);
                return Ok(response);
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