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
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetEventsByOrganisationId(int organisationId)
        {
            try
            {
                var events = _eventsService.GetEventsByOrganisationId(organisationId);
                return Ok(events);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch (Exception ex)
            {
                return BadRequest("Cannot process this request.");
            }
        }
        
        [HttpGet("api/organisations/{organisationId}/Events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(Event))]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public IActionResult GetEventById(int eventId, int organisationId)
        {
            var twEvent = _eventsService.GetEventByIdAndOrganisationId(eventId, organisationId);
            return twEvent switch
            {
                ForbidResult => StatusCode(403, "The event does not belong to this organisation"),
                NotFoundObjectResult => NotFound("Event not found"),
                _ => Ok(twEvent)
            };
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