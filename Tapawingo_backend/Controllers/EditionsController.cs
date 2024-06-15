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

        [HttpGet("events/{event_id}/editions/{edition_id}")]
        public IActionResult GetEditionById(int event_id, int edition_id)
        {
            try
            {
                var edition = _editionsService.GetEditionById(event_id, edition_id);
                return Ok(edition);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("events/{event_id}/editions")]
        public IActionResult GetEditions(int event_id)
        {
            try
            {
                var editions = _editionsService.GetAllEditions(event_id);
                return Ok(editions);

            }
            catch (Exception ex) 
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            
        }

        [HttpPost("events/{event_id}/editions")]
        public IActionResult CreateEdition([FromBody] CreateEditionDto model, int organisation_id, int event_id)
        {
            var twEvent = _editionsService.CreateEdition(model, organisation_id, event_id);
            return twEvent;
        }

        [HttpPatch("events/{eventId}/editions/{editionId}")]
        public async Task<IActionResult> UpdateEditionAsync(int eventId, int editionId, [FromBody] UpdateEditionDto model)
        {
            var response = await _editionsService.UpdateEditionAsync(eventId, editionId, model);
            return Ok(response);
        }

        [HttpDelete("events/{eventId}/editions/{editionId}")]
        public async Task<IActionResult> DeleteEditionAsync(int eventId, int editionId)
        {
            return await _editionsService.DeleteEditionAsync(eventId, editionId);
        }
    }
}
