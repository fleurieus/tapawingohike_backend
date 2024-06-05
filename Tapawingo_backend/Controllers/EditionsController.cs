using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Services;
using Tapawingo_backend.Dtos;

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
    
            [HttpPost("POST: organisations/{organisation_id}/events/{event_id}/editions")]
        public IActionResult CreateEdition([FromBody] CreateEditionDto model, int organisation_id, int event_id)
        {
            try
            {
                var response = _editionsService.CreateEdition(model, organisation_id, event_id);
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
