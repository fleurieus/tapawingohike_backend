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

        [Authorize(Policy = "SuperAdminOrOrganisationMPolicy")]
        [HttpGet("organisations/{organisationId}/users")]
        public async Task<IActionResult> GetUsersOnOrganisation(int organisationId)
        {
            return await _usersService.GetUsersOnOrganisation(organisationId);
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMPolicy")]
        [HttpGet("organisations/{organisationId}/user/{userId}")]
        public async Task<IActionResult> GetUserOnOrganisation(int organisationId, string userId)
        {
            var response =  await _usersService.GetUserOnOrganisationAsync(organisationId, userId);
            return Ok(response);
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMPolicy")]
        [HttpPost("organisations/{organisationId}/users")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUserOnOrganisation(int organisationId, [FromBody] CreateUserDto model)
        {
            var user = await _usersService.CreateUserOnOrganisation(organisationId, model);
            return new ObjectResult(user) { StatusCode = StatusCodes.Status201Created };
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMPolicy")] 
        [HttpPatch("organisations/{organisationId}/users/{userId}")]
        public async Task<IActionResult> UpdateUserOnOrganisation(int organisationId, string userId, [FromBody] UpdateUserDto userRegistrationDto)
        {
            var response = await _usersService.UpdateUserOnOrganisationAsync(organisationId, userId, userRegistrationDto);
            return Ok(response);
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMPolicy")]
        [HttpDelete("organisations/{organisationId}/users/{userId}")]
        public async Task<IActionResult> DeleteUserOnOrganisationAsync(int organisationId, string userId)
        {
            return await _usersService.DeleteUserOnOrganisationAsync(organisationId, userId);
        }


        // User on events
        [Authorize(Policy = "SuperAdminOrOrganisationMOrUPolicy")]
        [HttpGet("events/{eventId}/users")]
        public async Task<IActionResult> GetUsersOnEvent(int eventId)
        {
            return await _usersService.GetUsersOnEvent(eventId);
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUPolicy")]
        [HttpGet("events/{eventId}/users/{userId}")]
        public async Task<IActionResult> GetUserOnEvent(int eventId, string userId)
        {
            var response = await _usersService.GetUserOnEventAsync(eventId, userId);
            return Ok(response);
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUPolicy")]
        [HttpPost("events/{eventId}/users")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUserOnEvent(int eventId, [FromBody] CreateUserOnEventDto model)
        {
            var user = await _usersService.CreateUserOnEvent(eventId, model);
            return new ObjectResult(user) { StatusCode = StatusCodes.Status201Created };
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUPolicy")]
        [HttpPatch("events/{eventId}/users/{userId}")]
        public async Task<IActionResult> UpdateUserOnEvent(int eventId, string userId, [FromBody] UpdateUserOnEventDto userRegistrationDto)
        {
            var response = await _usersService.UpdateUserOnEventAsync(eventId, userId, userRegistrationDto);
            return Ok(response);
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUPolicy")]
        [HttpDelete("events/{eventId}/users/{userId}")]
        public async Task<IActionResult> DeleteUserOnEventAsync(int eventId, string userId)
        {
            return await _usersService.DeleteUserOnEventAsync(eventId, userId);
        }
    }
}
