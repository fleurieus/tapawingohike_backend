using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Controllers
{
    [ApiController]
    public class RoutepartsController : Controller
    {
        private readonly RoutepartsService _routepartsService;

        public RoutepartsController(RoutepartsService routepartsService) {
            _routepartsService = routepartsService;
        }

        [HttpGet("routes/{routeId}/routeparts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRouteParts(int routeId)
        {
            var route_parts = await _routepartsService.GetRoutepartsAsync(routeId);
            return route_parts == null ?
                new NotFoundObjectResult(new { message = "Route is not found" }) :
                new OkObjectResult(route_parts);
        }

        [HttpGet("routes/{routeId}/routeparts/{routepartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoutepartOnRoute(int routeId, int routepartId)
        {
            try
            {
                var response = await _routepartsService.GetRoutepartOnRouteAsync(routeId, routepartId);
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

        [HttpPost("routes/{routeId}/routeparts")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RoutepartDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateRoutepartAsync(int routeId, [FromForm] CreateRoutepartDto model)
        {
            var routepart = await _routepartsService.CreateRoutepartAsync(model, routeId);
            return routepart != null ?
                new ObjectResult(routepart) { StatusCode = StatusCodes.Status201Created } :
                NotFound(new { message = "Route not found" });
        }

        [HttpPut("routes/{routeId}/routeparts/{routepartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatRoutepartOnRouteAsync(int routeId, int routepartId, [FromForm] UpdateRoutepartDto UpdateRoutepartDto)
        {
            try
            {
                var response = await _routepartsService.UpdateRoutepartOnRouteAsync(routeId, routepartId, UpdateRoutepartDto);
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

        [HttpDelete("routes/{routeId}/routeparts/{routepartId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRoutepartOnRouteAsync(int routeId, int routepartId)
        {
            return await _routepartsService.DeleteRoutepartOnRouteAsync(routeId, routepartId);
        }
    }
}
