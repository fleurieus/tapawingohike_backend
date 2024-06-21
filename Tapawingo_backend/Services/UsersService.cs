using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Services
{
    public class UsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IOrganisationsRepository _organisationsRepository;
        private readonly IEventsRepository _eventsRepository;

        public UsersService(IUserRepository userRepository, IMapper mapper, IOrganisationsRepository organisationsRepository, IEventsRepository eventsRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _organisationsRepository = organisationsRepository;
            _eventsRepository = eventsRepository;
        }

        public IActionResult GetUsersOnOrganisation(int organisationId)
        {
            if (!_organisationsRepository.OrganisationExists(organisationId))
                return new NotFoundObjectResult(new
                {
                    message = "Organisation not found"
                });
            
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsersOnOrganisation(organisationId));
            return new OkObjectResult(users);
        }

        public async Task<UserDto> GetUserOnOrganisationAsync(int organisationId, string userId)
        {
            if (!_organisationsRepository.OrganisationExists(organisationId))
                throw new BadHttpRequestException("Organisation not found");

            if (!_userRepository.UserExists(userId))
                throw new BadHttpRequestException("User not found");

            if(!await _userRepository.UserExistsOnOrganisation(userId, organisationId))
                throw new BadHttpRequestException("User does not exist on organisation");
            

            return _mapper.Map<UserDto>(await _userRepository.GetUserOnOrganisationAsync(organisationId, userId));
        }

        public async Task<UserDto> CreateUserOnOrganisation(int organisationId, CreateUserDto model)
        {
            if (!_organisationsRepository.OrganisationExists(organisationId))
                throw new BadHttpRequestException("Organisation not found");

            var existingUser = _userRepository.GetUserByEmail(model.Email);

            if (existingUser != null)
                throw new BadHttpRequestException("User already exists");

            try
            {
                return _mapper.Map<UserDto>(await _userRepository.CreateUserOnOrganisation(organisationId, model));
            } catch (Exception e)
            {
                throw new BadHttpRequestException(e.Message);
            }
        }

        public async Task<UserDto> UpdateUserOnOrganisationAsync(int organisationId, string userId, UpdateUserDto user)
        {
            if (!_organisationsRepository.OrganisationExists(organisationId))
                throw new BadHttpRequestException("Organisation not found");

            if (await _userRepository.GetUserOnOrganisationAsync(organisationId, userId) == null)
                throw new BadHttpRequestException("User not found");
            if (!await _userRepository.UserExistsOnOrganisation(userId, organisationId))
                throw new BadHttpRequestException("User does not exist on organisation");
            await _userRepository.UpdateUserOnOrganisationAsync(await _userRepository.GetUserOnOrganisationAsync(organisationId, userId), user);

            return _mapper.Map<UserDto>(await _userRepository.GetUserOnOrganisationAsync(organisationId, userId));
        }

        public async Task<IActionResult> DeleteUserOnOrganisationAsync(int organisationId, string userId)
        {
            if (!_organisationsRepository.OrganisationExists(organisationId))
                return new NotFoundObjectResult(new
                {
                    message = "Organisation not found"
                });

            if (!_userRepository.UserExists(userId))
                return new NotFoundObjectResult(new
                {
                    message = "User not found"
                });
            if (!await _userRepository.UserExistsOnOrganisation(userId, organisationId))
                return new ConflictObjectResult(new
                {
                    message = "user does not exist on organisation"
                });

            bool userDeleted = await _userRepository.DeleteUserOnOrganisationAsync(organisationId, userId);
            return userDeleted ? new NoContentResult() : new BadRequestObjectResult(new
            {
                message = "User could not be deleted on organisation"
            });
        }

        // User on events
        public IActionResult GetUsersOnEvent(int eventId)
        {
            if (!_eventsRepository.EventExists(eventId))
                return new NotFoundObjectResult(new
                {
                    message = "Event not found"
                });

            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsersOnEvent(eventId));
            return new OkObjectResult(users);
        }

        public async Task<UserDto> GetUserOnEventAsync(int eventId, string userId)
        {
            if (!_eventsRepository.EventExists(eventId))
                throw new BadHttpRequestException("Event not found");

            if (!_userRepository.UserExists(userId))
                throw new BadHttpRequestException("User not found");
            if (!await _userRepository.UserExistsOnEvent(userId, eventId))
                throw new BadHttpRequestException("User does not exist on event");

            return _mapper.Map<UserDto>(await _userRepository.GetUserOnEventAsync(eventId, userId));
        }

        public async Task<UserDto> CreateUserOnEvent(int eventId, CreateUserOnEventDto model)
        {
            if (!_eventsRepository.EventExists(eventId))
                throw new BadHttpRequestException("Event not found");

            var existingUser = _userRepository.GetUserByEmail(model.Email);

            if (existingUser != null)
                throw new BadHttpRequestException("User already exists");

            try
            {
                return _mapper.Map<UserDto>(await _userRepository.CreateUserOnEvent(eventId, model));
            }
            catch (Exception e)
            {
                throw new BadHttpRequestException(e.Message);
            }
        }

        public async Task<UserDto> UpdateUserOnEventAsync(int eventId, string userId, UpdateUserOnEventDto user)
        {
            if (!_eventsRepository.EventExists(eventId))
                throw new BadHttpRequestException("Event not found");

            if (await _userRepository.GetUserOnEventAsync(eventId, userId) == null)
                throw new BadHttpRequestException("User not found");
            if (!await _userRepository.UserExistsOnEvent(userId, eventId))
                throw new BadHttpRequestException("User does not exist on event");

            await _userRepository.UpdateUserOnEventAsync(await _userRepository.GetUserOnEventAsync(eventId, userId), user);

            return _mapper.Map<UserDto>(await _userRepository.GetUserOnEventAsync(eventId, userId));
        }

        public async Task<IActionResult> DeleteUserOnEventAsync(int eventId, string userId)
        {
            if (!_eventsRepository.EventExists(eventId))
                return new NotFoundObjectResult(new
                {
                    message = "Event not found"
                });

            if (!_userRepository.UserExists(userId))
                return new NotFoundObjectResult(new
                {
                    message = "User not found"
                });
            if (!await _userRepository.UserExistsOnEvent(userId, eventId))
                return new ConflictObjectResult( new
                {
                    message = "User does not exist on event"
                });

            bool userDeleted = await _userRepository.DeleteUserOnEventAsync(eventId, userId);
            return userDeleted ? new NoContentResult() : new BadRequestObjectResult(new
            {
                message = "User could not be deleted on event"
            });
        }
    }
}
