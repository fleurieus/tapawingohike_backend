using AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;
using Xunit;

namespace Tapawingo_backend.Tests.TEST_Teams.PATCH_Team
{
    [Collection("Database collection")]
    public class Teams_Service_Tests : TestBase
    {
        private readonly TeamRepository _teamRepository;
        private readonly TeamService _teamsService;
        private readonly EditionsRepository _editionsRepository;
        private readonly RoutepartsRepository _routepartsRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Teams_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context;
            _teamRepository = new TeamRepository(_context);
            _editionsRepository = new EditionsRepository(_context);
            _routepartsRepository = new RoutepartsRepository(_context);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });
            _mapper = config.CreateMapper();

            _teamsService = new TeamService(_teamRepository, _editionsRepository, _routepartsRepository, _mapper);
        }

        // Good Weather: Test for creating a team successfully
        [Fact]
        public async Task Patch_team()
        {
            UpdateTeamDto updateTeamDto = new UpdateTeamDto
            {
                Name = "Test Team",
                Code = "TST001",
                ContactName = "John Doe",
                ContactEmail = "john.doe@example.com",
                ContactPhone = "1234567890",
                Online = false
            };

            var teamDto = await _teamsService.UpdateTeamOnEditionAsync(1, 2, updateTeamDto);

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
