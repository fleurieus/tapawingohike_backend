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

namespace Tapawingo_backend.Tests
{
    [Collection("Database collection")]
    public class Teams_Service_Tests : TestBase
    {
        private readonly TeamRepository _teamRepository;
        private readonly TeamService _teamsService;
        private readonly EditionsRepository _editionsRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Teams_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context;
            _teamRepository = new TeamRepository(_context);
            _editionsRepository = new EditionsRepository(_context);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });
            _mapper = config.CreateMapper();

            _teamsService = new TeamService(_teamRepository, _editionsRepository, _mapper);
        }

        // Good Weather: Test for creating a team successfully
        [Fact]
        public async Task Create_Team_Successful()
        {
            var createTeamDto = new CreateTeamDto
            {
                Name = "Test Team",
                Code = "TST001",
                ContactName = "John Doe",
                ContactEmail = "john.doe@example.com",
                ContactPhone = "1234567890",
                Online = true
            };

            var teamDto = await _teamsService.CreateTeamOnEdition(1, createTeamDto);

            Assert.NotNull(teamDto);
            Assert.Equal(createTeamDto.Name, teamDto.Name);
            Assert.Equal(createTeamDto.Code, teamDto.Code);
            Assert.Equal(createTeamDto.ContactName, teamDto.ContactName);
            Assert.Equal(createTeamDto.ContactEmail, teamDto.ContactEmail);
            Assert.Equal(createTeamDto.ContactPhone, teamDto.ContactPhone);
            Assert.Equal(createTeamDto.Online, teamDto.Online);
            Assert.Equal(1, teamDto.EditionId);
        }

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
