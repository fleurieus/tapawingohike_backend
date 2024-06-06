using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Controllers
{
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("api/organisations/{organisationId}/users")]
        public IActionResult GetUsersOnOrganisation(int organisationId)
        {
            return _usersService.GetUsersOnOrganisation(organisationId);
        }

        [HttpGet("api/organisations/{organisationId}/user/{userId}")]
        public IActionResult GetUserOnOrganisation(int organisationId, string userId)
        {
            return _usersService.GetUserOnOrganisation(organisationId, userId);
        }

        [HttpPost("api/organisations/{organisationId}/users")]
        public async Task<IActionResult> CreateUserOnOrganisation(int organisationId, [FromBody] CreateUserDto model)
        {
            var response = await _usersService.CreateUserOnOrganisation(organisationId, model);
            return Ok(response);
        }

        [HttpPatch("api/organisations/{organisationId}/users/{userId}")]
        public async Task<IActionResult> UpdateUserOnOrganisation(int organisationId, string userId, [FromBody] UpdateUserDto userRegistrationDto)
        {
            var response = await _usersService.UpdateUserOnOrganisationAsync(organisationId, userId, userRegistrationDto);
            return Ok(response);
        }

        [HttpDelete("api/organisations/{organisationId}/users/{userId}")]
        public IActionResult DeleteUserOnOrganisation(int organisationId, string userId)
        {
            return _usersService.DeleteUserOnOrganisation(organisationId, userId);
        }
    }
}
