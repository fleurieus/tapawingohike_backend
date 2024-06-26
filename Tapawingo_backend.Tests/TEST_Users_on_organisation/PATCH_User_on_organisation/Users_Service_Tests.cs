using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.TEST_Users_on_organisation.PATCH_User_on_organisation
{
    [Collection("Database collection")]
    public class Users_Service_Tests : TestBase
    {
        private readonly UsersRepository _usersRepository;
        private readonly UsersService _usersService;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IOrganisationsRepository> _organisationsRepositoryMock;
        private readonly Mock<IEventsRepository> _eventsRepositoryMock;

        public Users_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context;

            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            _organisationsRepositoryMock = new Mock<IOrganisationsRepository>();
            _eventsRepositoryMock = new Mock<IEventsRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });
            _mapper = config.CreateMapper();

            _usersRepository = new UsersRepository(_context, _userManagerMock.Object);

            _usersService = new UsersService(_usersRepository, _mapper, _organisationsRepositoryMock.Object, _eventsRepositoryMock.Object, _context);
        }

        //Good Weather
        [Fact]
        public async Task Update_User_On_Organisation()
        {
            var users = await _usersRepository.GetUsersOnOrganisation(1);
            var firstUser = users.First();

            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                LastName = "updateTest",
            };

            var user = await _usersRepository.UpdateUserOnOrganisationAsync(firstUser, updateUserDto);
            Assert.Equal("updateTest", user.LastName);
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
