﻿using AutoMapper;
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

namespace Tapawingo_backend.Tests.POST_User_on_organisation
{
    [Collection("Database collection")]
    public class Users_Service_Tests : TestBase
    {
        private readonly UsersRepository _usersRepository;
        private readonly UsersService _usersService;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly Mock<IOrganisationsRepository> _organisationsRepositoryMock;

        public Users_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context;

            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStoreMock.Object, null, null, null, null);

            _organisationsRepositoryMock = new Mock<IOrganisationsRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });
            _mapper = config.CreateMapper();

            _usersRepository = new UsersRepository(_context, _userManagerMock.Object, _roleManagerMock.Object);

            _usersService = new UsersService(_usersRepository, _mapper, _organisationsRepositoryMock.Object);
        }

        //Good Weather
        [Fact]
        public void Create_User_On_Organisation()
        {
            CreateUserDto createUserDto = new CreateUserDto
            {
                FirstName = "test",
                LastName = "test",
                Email = "test99@gmail.nl",
                Password = "Password!1"
            };

            var user = _usersRepository.CreateUserOnOrganisation(1, createUserDto);

            Assert.NotNull(user);

            //var organisationUsers = _usersRepository.GetUsersOnOrganisation(1);
            //Assert.Equal(3, organisationUsers.Count());
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
