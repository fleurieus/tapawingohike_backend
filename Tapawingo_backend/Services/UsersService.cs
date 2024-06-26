using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Tapawingo_backend.Data;
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
        private readonly DataContext _context;

        public UsersService(IUserRepository userRepository, IMapper mapper, IOrganisationsRepository organisationsRepository, IEventsRepository eventsRepository, DataContext dataContext)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _organisationsRepository = organisationsRepository;
            _eventsRepository = eventsRepository;
            _context = dataContext;
        }

        public async Task<IActionResult> GetUsersOnOrganisation(int organisationId)
        {
            if (!await _organisationsRepository.OrganisationExists(organisationId))
                return new NotFoundObjectResult(new
                {
                    message = "Organisation not found"
                });

            var users = await _userRepository.GetUsersOnOrganisation(organisationId);

            var usersWithRole = new List<UserOnOrganisationDto>();
            foreach (var user in users)
            {
                var userOnOrganisation = await _context.UserOrganisations.FirstOrDefaultAsync(uo => uo.UserId == user.Id && uo.OrganisationId == organisationId);
                usersWithRole.Add(new UserOnOrganisationDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    IsManager = userOnOrganisation.IsManager
                });
            }

            return new OkObjectResult(usersWithRole);
        }

        public async Task<UserOnOrganisationDto> GetUserOnOrganisationAsync(int organisationId, string userId)
        {
            if (!await _organisationsRepository.OrganisationExists(organisationId))
                throw new BadHttpRequestException("Organisation not found");

            if (!await _userRepository.UserExists(userId))
                throw new BadHttpRequestException("User not found");

            if(!await _userRepository.UserExistsOnOrganisation(userId, organisationId))
                throw new BadHttpRequestException("User does not exist on organisation");

            var user = await _userRepository.GetUserOnOrganisationAsync(organisationId, userId);

            var userOnOrganisation = await _context.UserOrganisations.FirstOrDefaultAsync(uo => uo.UserId == userId && uo.OrganisationId == organisationId);
            var userWithRole = new UserOnOrganisationDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsManager = userOnOrganisation.IsManager
            };

            return userWithRole;
        }

        public async Task<UserOnOrganisationDto> CreateUserOnOrganisation(int organisationId, CreateUserDto model)
        {
            if (!await _organisationsRepository.OrganisationExists(organisationId))
                throw new BadHttpRequestException("Organisation not found");

            var existingUser = await _userRepository.GetUserByEmail(model.Email);

            if (existingUser != null)
                throw new BadHttpRequestException("User already exists");

            try
            {
                var newUser = _mapper.Map<UserDto>(await _userRepository.CreateUserOnOrganisation(organisationId, model));
                var userOnOrganisation = await _context.UserOrganisations.FirstOrDefaultAsync(uo => uo.UserId == newUser.Id && uo.OrganisationId == organisationId);
                var userWithRole = new UserOnOrganisationDto()
                {
                    Id = newUser.Id,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Email = newUser.Email,
                    IsManager = userOnOrganisation.IsManager
                };

                return userWithRole;
            } catch (Exception e)
            {
                throw new BadHttpRequestException(e.Message);
            }
        }

        public async Task<UserOnOrganisationDto> UpdateUserOnOrganisationAsync(int organisationId, string userId, UpdateUserDto user)
        {
            if (!await _organisationsRepository.OrganisationExists(organisationId))
                throw new BadHttpRequestException("Organisation not found");

            if (await _userRepository.GetUserOnOrganisationAsync(organisationId, userId) == null)
                throw new BadHttpRequestException("User not found");
            if (!await _userRepository.UserExistsOnOrganisation(userId, organisationId))
                throw new BadHttpRequestException("User does not exist on organisation");
            await _userRepository.UpdateUserOnOrganisationAsync(await _userRepository.GetUserOnOrganisationAsync(organisationId, userId), user);

            var updatedUser = await _userRepository.GetUserOnOrganisationAsync(organisationId, userId);
            var userOnOrganisation = await _context.UserOrganisations.FirstOrDefaultAsync(uo => uo.UserId == userId && uo.OrganisationId == organisationId);
            var userWithRole = new UserOnOrganisationDto()
            {
                Id = updatedUser.Id,
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
                Email = updatedUser.Email,
                IsManager = userOnOrganisation.IsManager
            };

            return userWithRole;
        }

        public async Task<IActionResult> DeleteUserOnOrganisationAsync(int organisationId, string userId)
        {
            if (!await _organisationsRepository.OrganisationExists(organisationId))
                return new NotFoundObjectResult(new
                {
                    message = "Organisation not found"
                });

            if (!await _userRepository.UserExists(userId))
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
        public async Task<IActionResult> GetUsersOnEvent(int eventId)
        {
            if (!await _eventsRepository.EventExists(eventId))
                return new NotFoundObjectResult(new
                {
                    message = "Event not found"
                });

            var users = _mapper.Map<List<UserDto>>(await _userRepository.GetUsersOnEvent(eventId));
            return new OkObjectResult(users);
        }

        public async Task<UserDto> GetUserOnEventAsync(int eventId, string userId)
        {
            if (!await _eventsRepository.EventExists(eventId))
                throw new BadHttpRequestException("Event not found");

            if (!await _userRepository.UserExists(userId))
                throw new BadHttpRequestException("User not found");
            if (!await _userRepository.UserExistsOnEvent(userId, eventId))
                throw new BadHttpRequestException("User does not exist on event");

            return _mapper.Map<UserDto>(await _userRepository.GetUserOnEventAsync(eventId, userId));
        }

        public async Task<UserDto> CreateUserOnEvent(int eventId, CreateUserOnEventDto model)
        {
            if (!await _eventsRepository.EventExists(eventId))
                throw new BadHttpRequestException("Event not found");

            var existingUser = await _userRepository.GetUserByEmail(model.Email);

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
            if (!await _eventsRepository.EventExists(eventId))
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
            if (!await _eventsRepository.EventExists(eventId))
                return new NotFoundObjectResult(new
                {
                    message = "Event not found"
                });

            if (!await _userRepository.UserExists(userId))
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
