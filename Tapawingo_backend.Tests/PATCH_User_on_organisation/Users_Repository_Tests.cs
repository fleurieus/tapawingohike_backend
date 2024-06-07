using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Tests.PATCH_User_on_organisation
{
    [Collection("Database collection")]
    public class Users_Repository_Tests : TestBase
    {
        private readonly UsersRepository _usersRepository;
        private readonly DataContext _context;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;

        public Users_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context;

            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStoreMock.Object, null, null, null, null);

            _usersRepository = new UsersRepository(_context, _userManagerMock.Object, _roleManagerMock.Object);
        }

        //Good Weather
        [Fact]
        public void Update_User_On_Organisation()
        {
            var users = _usersRepository.GetUsersOnOrganisation(1);
            var firstUser = users.First();

            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                LastName = "updateTest",
            };

            var user = _usersRepository.UpdateUserOnOrganisationAsync(firstUser, updateUserDto);

            Assert.NotNull(user);
            var updatedUser = _usersRepository.GetUserOnOrganisation(1, firstUser.Id);
            Assert.Equal("updateTest", updatedUser.LastName);
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
