using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Services;
using Tapawingo_backend.Dtos;
using Microsoft.Extensions.Logging;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Controllers
{
    [ApiController]
    public class EditionsController : Controller
    {
        private readonly EditionsService _editionsService;

        public EditionsController(EditionsService editionsService)
        {
            _editionsService = editionsService;
        }

        [HttpGet("events/{event_id}/editions")]
        public IActionResult GetEditions(int event_id)
        {
            return Ok(_editionsService.GetAllEditions(event_id));
        }

        [HttpPost("events/{event_id}/editions")]
        public IActionResult CreateEdition([FromBody] CreateEditionDto model, int organisation_id, int event_id)
        {
            var twEvent = _editionsService.CreateEdition(model, organisation_id, event_id);
            return twEvent;
        }
    }
}
