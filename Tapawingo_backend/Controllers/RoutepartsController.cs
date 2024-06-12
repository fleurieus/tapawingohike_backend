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

        [HttpPost("routes/{route_id}/route_parts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoutepartDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateRoutepart([FromBody] CreateRoutepartDto model, int route_id)
        {
            var routepart = await _routepartsService.CreateRoutepart(model, route_id);
            return routepart != null ?
                new ObjectResult(routepart) { StatusCode = StatusCodes.Status201Created } :
                NotFound(new { message = "Routepart not found" });
        }
    }
}
