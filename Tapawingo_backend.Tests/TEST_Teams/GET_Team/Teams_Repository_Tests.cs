using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;
using Tapawingo_backend.Data;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.TEST_Teams.GET_Team
{
    [Collection("Database collection")]
    public class Teams_Repository_Tests : TestBase
    {
        private readonly TeamRepository _teamsRepository;
        private readonly DataContext _context;

        public Teams_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context;
            _teamsRepository = new TeamRepository(_context);
        }

        // Good Weather
        [Fact]
        public async Task Get_Team()
        {
            var team = await _teamsRepository.GetTeamOnEditionAsync(1, 2);

            Assert.NotNull(team);
            Assert.Equal("TestTeam2", team.Name);
        }

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
