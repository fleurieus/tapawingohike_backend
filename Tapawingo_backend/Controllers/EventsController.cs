using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Models;
using Tapawingo_backend.Services;
using System.Linq;

namespace Tapawingo_backend.Controllers
{
    [Route("api/organisations/{organisationId}/[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly EventsService _eventsService;

        public EventsController(EventsService eventsService)
        {
            _eventsService = eventsService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Event>))]
        public IActionResult GetEventsByOrganisationId(int organisationId)
        {
            var events = _eventsService.GetEventsByOrganisationId(organisationId);
            return Ok(events);
        }
        
        [HttpGet("{eventId}")]
        [ProducesResponseType(200, Type = typeof(Event))]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public IActionResult GetEventById(int eventId, int organisationId)
        {
            var twEvent = _eventsService.getEventByIdAndOrganisationId(eventId, organisationId);
            return twEvent switch
            {
                ForbidResult => StatusCode(403, "The event does not belong to this organisation"),
                NotFoundObjectResult => NotFound("Event not found"),
                _ => Ok(twEvent)
            };
        }
    }
}