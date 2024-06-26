using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Services;
using Tapawingo_backend.Dtos;
using Microsoft.Extensions.Logging;
using Tapawingo_backend.Models;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet("events/{eventId}/editions/{editionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEditionById(int eventId, int editionId)
        {
            try
            {
                var edition = await _editionsService.GetEditionById(eventId, editionId);
                return Ok(edition);
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("events/{eventId}/editions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEditions(int eventId)
        {
            try
            {
                var editions = await _editionsService.GetAllEditions(eventId);
                return Ok(editions);

            }
            catch (Exception ex) 
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            
        }

        [HttpPost("events/{eventId}/editions")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EditionDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateEditionOnEventAsync(int eventId, [FromBody] CreateEditionDto model)
        {
            try
            {
                var edition = await _editionsService.CreateEditionOnEventAsync(eventId, model);
                return new ObjectResult(edition) { StatusCode = StatusCodes.Status201Created };
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(new
                {
                    message = ex.Message
                });
            }
            
        }

        [HttpPatch("events/{eventId}/editions/{editionId}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EditionDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEditionAsync(int eventId, int editionId, [FromBody] UpdateEditionDto model)
        {
            try
            {
                var response = await _editionsService.UpdateEditionAsync(eventId, editionId, model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(new
                {
                    message = ex.Message
                });
            }
            
        }

        [HttpDelete("events/{eventId}/editions/{editionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEditionAsync(int eventId, int editionId)
        {
            try
            {
                return await _editionsService.DeleteEditionAsync(eventId, editionId);
            }
            catch (Exception ex)
            {
                return new NotFoundObjectResult(new
                {
                    message = ex.Message
                });
            }
            
        }
    }
}
