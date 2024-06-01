using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : Controller
    {
        private readonly RoutesService _routesService;

        public RoutesController(RoutesService routesService)
        {
            _routesService = routesService;
        }

        //TODO: ADD AUTHORIZATION RULE
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RouteDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetRoute(int id)
        {
            var organisation = _routesService.GetRoutesById(id);
            return organisation != null ?
                Ok(organisation) :
                NotFound("Organisation with this id was not found.");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RouteDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateRoute([FromBody] CreateRouteDto createRouteDto)
        {
            var newRoute = _routesService.CreateRoute(createRouteDto);
            if (newRoute != null)
            {
                return new ObjectResult(newRoute)
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            return BadRequest("Cannot process this request.");
        }
    }
}
