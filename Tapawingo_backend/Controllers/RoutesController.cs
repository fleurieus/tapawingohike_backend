using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Controllers
{
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
    }
}
