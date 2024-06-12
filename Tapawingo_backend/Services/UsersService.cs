using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public UsersService(IUserRepository userRepository, IMapper mapper, IOrganisationsRepository organisationsRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _organisationsRepository = organisationsRepository;
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

            bool userDeleted = await _userRepository.DeleteUserOnOrganisationAsync(organisationId, userId);
            return userDeleted ? new NoContentResult() : new BadRequestObjectResult(new
            {
                message = "User could not be deleted"
            });
        }
    }
}
