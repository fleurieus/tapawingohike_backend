using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Controllers
{
    [ApiController]
    public class OrganisationsController : ControllerBase
    {
        private readonly OrganisationsService _organisationsService;

        public OrganisationsController(OrganisationsService organisationsService)
        {
            _organisationsService = organisationsService;
        }

        [Authorize(Policy = "SuperAdminPolicy")]
        [HttpGet("organisations/")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrganisationDto))]
        public async Task<IActionResult> GetOrganisations()
        {
            var organisations = await _organisationsService.GetOrganisations();
            return organisations == null ? Ok(new List<OrganisationDto>()) : Ok(organisations); //since the request issn't invalid, even a empty list gives a 200 status
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUOrEventUserPolicy")]
        [HttpGet("organisations/{organisationId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrganisationDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrganisation(int organisationId)
        {
            var organisation = await _organisationsService.GetOrganisationById(organisationId);
            return organisation != null ?
                Ok(organisation) :
                NotFound("Organisation with this id was not found.");
        }

        [Authorize(Policy = "SuperAdminPolicy")]
        [HttpPost("organisations/")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrganisationDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrganisation([FromBody]CreateOrganisationDto model)
        {
            if(model.Name == null || model.Name.Length == 0 || model.Name.Equals(""))
            {
                return BadRequest("Cannot create organisation without name.");
            }

            //only doing these 'extra' steps since we want to return a 201 status with the object.
            var newOrganisation = await _organisationsService.CreateOrganisation(model);
            if(newOrganisation != null)
            {
                return new ObjectResult(newOrganisation)
                {
                    StatusCode = StatusCodes.Status201Created
                };
            }
            return BadRequest("Cannot process this request.");
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMPolicy")]
        [HttpPatch("organisations/{organisationId}")]
        public async Task<IActionResult> UpdateOrganisation(int organisationId, [FromBody]UpdateOrganisationDto model) 
        {
            var updatedOrganisation = await _organisationsService.UpdateOrganisation(organisationId, model);
            return updatedOrganisation == null ?
                NotFound(new
                {
                    message = "This organisation could not be found."
                }) :
                Ok(updatedOrganisation);
        }

        [Authorize(Policy = "SuperAdminPolicy")]
        [HttpDelete("organisations/{organisationId}")]
        public async Task<IActionResult> DeleteOrganisation(int organisationId)
        {
            return await _organisationsService.DeleteOrganisationAsync(organisationId);
        }
    }
}
