using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Models;
using Tapawingo_backend.Services;
using System.Linq;

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
            try
            {
                var events = _eventsService.GetEventsByOrganisationId(organisationId);
                return Ok(events);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); 
            }
        }
    }
}