using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganisationsController : ControllerBase
    {
        private readonly OrganisationsService _organisationsService;

        public OrganisationsController(OrganisationsService organisationsService)
        {
            _organisationsService = organisationsService;
        }

        //TODO: ADD AUTHORIZATION RULE
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrganisationDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetOrganisation(int id)
        {
            var organisation = _organisationsService.GetOrganisationById(id);
            return organisation!=null ? 
                Ok(organisation) : 
                NotFound("Organisation with this id was not found.");
        }

        //TODO: ADD AUTHORIZATION RULE
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrganisationDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateOrganisation([FromBody]CreateOrganisationDto name)
        {
            if(name.Name == null || name.Name.Length == 0 || name.Name.Equals(""))
            {
                return BadRequest("Cannot create organisation without name.");
            }

            //only doing these 'extra' steps since we want to return a 201 status with the object.
            var newOrganisation = _organisationsService.CreateOrganisation(name);
            if(newOrganisation != null)
            {
                return new ObjectResult(newOrganisation)
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            return BadRequest("Cannot process this request.");
        }
    }
}
