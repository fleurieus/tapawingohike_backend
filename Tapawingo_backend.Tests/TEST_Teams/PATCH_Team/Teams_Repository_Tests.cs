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

namespace Tapawingo_backend.Tests.TEST_Teams.PATCH_Team
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
        public async Task Patch_team()
        {
            Team team = await _teamsRepository.GetTeamOnEditionAsync(1, 2);
            
            UpdateTeamDto updateTeamDto = new UpdateTeamDto
            {
                Name = "Test Team",
                Code = "TST001",
                ContactName = "John Doe",
                ContactEmail = "john.doe@example.com",
                ContactPhone = "1234567890",
                Online = false
            };

            var teamDto = await _teamsRepository.UpdateTeamOnEditionAsync(team, updateTeamDto);

            Assert.NotNull(teamDto);
            Assert.Equal(updateTeamDto.Name, teamDto.Name);
            Assert.Equal(updateTeamDto.Code, teamDto.Code);
            Assert.Equal(updateTeamDto.ContactName, teamDto.ContactName);
            Assert.Equal(updateTeamDto.ContactEmail, teamDto.ContactEmail);
            Assert.Equal(updateTeamDto.ContactPhone, teamDto.ContactPhone);
            Assert.Equal(updateTeamDto.Online, teamDto.Online);
        }

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
