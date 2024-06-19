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
        public async Task<IActionResult> GetRouteParts(int routeId)
        {
            var route_parts = await _routepartsService.GetRoutepartsAsync(routeId);
            return route_parts == null ?
                new NotFoundObjectResult(new { message = "Route is not found" }) :
                new OkObjectResult(route_parts);
        }

        [HttpGet("routes/{routeId}/routeparts/{routepartId}")]
        public async Task<IActionResult> GetRoutepartOnRoute(int routeId, int routepartId)
        {
            var response = await _routepartsService.GetRoutepartOnRouteAsync(routeId, routepartId);
            return Ok(response);
        }

        [HttpPost("routes/{routeId}/routeparts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoutepartDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateRoutepartAsync(int routeId, [FromForm] CreateRoutepartDto model)
        {
            var routepart = await _routepartsService.CreateRoutepartAsync(model, routeId);
            return routepart != null ?
                new ObjectResult(routepart) { StatusCode = StatusCodes.Status201Created } :
                NotFound(new { message = "Route not found" });
        }
    }
}
