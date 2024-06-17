using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Controllers
{
    [ApiController]
    public class RoutesController : Controller
    {
        private readonly RoutesService _routesService;

        public RoutesController(RoutesService routesService)
        {
            _routesService = routesService;
        }

        [HttpGet("editions/{editionId}/routes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RouteDto>))]
        public async Task<IActionResult> GetRoutes(int editionId)
        {
            try
            {
                var routes = await _routesService.GetRoutes(editionId);
                return routes == null ?
                    Ok(new List<RouteDto>()) :
                    Ok(routes);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new
                {
                    message = "This edition has not been found"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "This message could not been handled",
                    internalMessage = ex.Message
                });
            }
        }


        //TODO: ADD AUTHORIZATION RULE
        [HttpGet("editions/{editionId}/routes/{routeId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RouteDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoute(int editionId, int routeId)
        {
            try
            {
                var route = await _routesService.GetRoutesById(editionId, routeId);
                return route != null ?
                    Ok(route) :
                    NotFound(new { message = "Route not found" });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "This message could not been handled",
                    internalMessage = ex.Message
                });
            }
        }

        [HttpDelete("editions/{editionId}/routes/{routeId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteRoute(int editionId, int routeId)
        {
            try
            {
                var route = await _routesService.DeleteRouteById(editionId, routeId);
                return route ?
                    NoContent() :
                    BadRequest(new { message = "Request could not been handled" });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "This message could not been handled",
                    internalMessage = ex.Message
                });
            }
        }
    }
}
