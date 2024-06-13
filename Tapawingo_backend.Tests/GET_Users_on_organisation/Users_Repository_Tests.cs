using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Tests.GET_Users_on_organisation
{
    [Collection("Database collection")]
    public class User_Repository_Tests : TestBase
    {
        private readonly UsersRepository _usersRepository;
        private readonly DataContext _context;
        private readonly Mock<UserManager<User>> _userManagerMock;

        public User_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context;

            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            _usersRepository = new UsersRepository(_context, _userManagerMock.Object);
        }

        //Good Weather
        [Fact]
        public void Get_All_Users_On_Organisation()
        {
            var users = _usersRepository.GetUsersOnOrganisation(1);

            Assert.NotNull(users);
            Assert.Equal(2, users.Count());
            Assert.Equal("test1@gmail.com", users.First().Email);
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
