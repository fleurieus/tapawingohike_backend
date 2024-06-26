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

        [HttpGet("organisations/{organisationId}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsersOnOrganisation(int organisationId)
        {
            return await _usersService.GetUsersOnOrganisation(organisationId);
        }

        [HttpGet("organisations/{organisationId}/user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserOnOrganisation(int organisationId, string userId)
        {
            try
            {
                var response =  await _usersService.GetUserOnOrganisationAsync(organisationId, userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize(Policy = "SuperAdminOrOrganisationPolicy")]
        [HttpPost("organisations/{organisationId}/users")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateUserOnOrganisation(int organisationId, [FromBody] CreateUserDto model)
        {
            try
            {
                var user = await _usersService.CreateUserOnOrganisation(organisationId, model);
                return new ObjectResult(user) { StatusCode = StatusCodes.Status201Created };
            }
            catch(BadHttpRequestException ex)
            {
                return new BadRequestObjectResult(new
                {
                    message = ex.Message
                });
            }
            catch(ArgumentException ex)
            {
                return new NotFoundObjectResult(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPatch("organisations/{organisationId}/users/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserOnOrganisation(int organisationId, string userId, [FromBody] UpdateUserDto userRegistrationDto)
        {
            var response = await _usersService.UpdateUserOnOrganisationAsync(organisationId, userId, userRegistrationDto);
            return Ok(response);
        }

        [HttpDelete("organisations/{organisationId}/users/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteUserOnOrganisationAsync(int organisationId, string userId)
        {
            return await _usersService.DeleteUserOnOrganisationAsync(organisationId, userId);
        }


        // User on events
        [HttpGet("events/{eventId}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsersOnEvent(int eventId)
        {
            return await _usersService.GetUsersOnEvent(eventId);
        }

        [HttpGet("events/{eventId}/users/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserOnEvent(int eventId, string userId)
        {
            try
            {
                var response = await _usersService.GetUserOnEventAsync(eventId, userId);
                return Ok(response);
            }
            catch (Exception ex) {
                return new NotFoundObjectResult(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize(Policy = "SuperAdminOrOrganisationMOrUPolicy")]
        [HttpPost("events/{eventId}/users")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateUserOnEvent(int eventId, [FromBody] CreateUserOnEventDto model)
        {
            try
            {
                var user = await _usersService.CreateUserOnEvent(eventId, model);
                return new ObjectResult(user) { StatusCode = StatusCodes.Status201Created };
            }
            catch (ArgumentException ex)
            {
                return new NotFoundObjectResult(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPatch("events/{eventId}/users/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserOnEvent(int eventId, string userId, [FromBody] UpdateUserOnEventDto userRegistrationDto)
        {
            try
            {
                var response = await _usersService.UpdateUserOnEventAsync(eventId, userId, userRegistrationDto);
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

        [HttpDelete("events/{eventId}/users/{userId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteUserOnEventAsync(int eventId, string userId)
        {
            return await _usersService.DeleteUserOnEventAsync(eventId, userId);
        }
    }
}
