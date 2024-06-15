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

namespace Tapawingo_backend.Tests.TEST_Teams.GET_Teams
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
        public async Task GET_teams()
        {
            var teams = _teamsRepository.GetTeamsOnEdition(1);

            Assert.NotNull(teams);
            Assert.Equal(2, teams.Count());
        }

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
