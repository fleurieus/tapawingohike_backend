using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IUserRepository userRepository, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());
            return new ObjectResult(users);
        }

        public IActionResult GetUser(string userId)
        {
            if (!_userRepository.UserExists(userId))
                return new NotFoundObjectResult(new
                {
                    message = "User not found"
                });

            var user = _mapper.Map<UserDto>(_userRepository.GetUser(userId));

            return new ObjectResult(user);
        }

        public async Task<CustomResponse> CreateUser(CreateUserDto model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
                throw new BadHttpRequestException("User already exists");

            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var createUser = await _userManager.CreateAsync(newUser, model.Password);
            if (!createUser.Succeeded)
            {
                var errorMessages = string.Join(", ", createUser.Errors.Select(e => e.Description));
                throw new DetailedIdentityErrorException(Status.UnProcessableEntity, errorMessages);
            }

            //Assign Default Role : Admin to first registrar; rest is user
            var checkAdmin = await _roleManager.FindByNameAsync("SuperAdmin");
            if (checkAdmin is null)
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = "SuperAdmin" });
                await _userManager.AddToRoleAsync(newUser, "SuperAdmin");
                return new CustomResponse("User created");
            }
            else
            {
                var checkUser = await _roleManager.FindByNameAsync("User");
                if (checkUser is null)
                    await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });

                await _userManager.AddToRoleAsync(newUser, "User");
                return new CustomResponse("User created");
            }
        }

        public async Task<IActionResult> UpdateUserAsync(string userId, UpdateUserDto user)
        {
            if (_userRepository.GetUser(userId) == null)
                return new NotFoundObjectResult(new { message = "The desired user is not found" });

            await _userRepository.TryUpdateUser(_userRepository.GetUser(userId), user);

            var updatedUser = _mapper.Map<UserDto>(_userRepository.GetUser(userId));

            return new ObjectResult(updatedUser);
        }

        public IActionResult DeleteUser(string userId)
        {
            bool userDeleted = _userRepository.DeleteUser(userId);
            return userDeleted ? new NoContentResult() : new BadRequestObjectResult(new
            {
                message = "User could not be deleted"
            });
        }
    }
}
