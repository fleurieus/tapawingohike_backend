using AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;
using Xunit;

namespace Tapawingo_backend.Tests
{
    [Collection("Database collection")]
    public class Teams_Service_Tests : TestBase, IDisposable
    {
        private readonly TeamRepository _teamRepository;
        private readonly TeamService _teamsService;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Teams_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; // inject 'shared' context from TestBase
            _teamRepository = new TeamRepository(_context);
            
            // Create an instance of the IMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateTeamDto, Team>();
                cfg.CreateMap<Team, CreateTeamDto>();
            });
            _mapper = config.CreateMapper();

            // Create the service instance
            _teamsService = new TeamService(_teamRepository, _mapper);
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
                Online = true,
                EditionId = 1
            };

            var teamDto = await _teamsService.CreateTeam(createTeamDto);

            Assert.NotNull(teamDto);
            Assert.Equal(createTeamDto.Name, teamDto.Name);
            Assert.Equal(createTeamDto.Code, teamDto.Code);
            Assert.Equal(createTeamDto.ContactName, teamDto.ContactName);
            Assert.Equal(createTeamDto.ContactEmail, teamDto.ContactEmail);
            Assert.Equal(createTeamDto.ContactPhone, teamDto.ContactPhone);
            Assert.Equal(createTeamDto.Online, teamDto.Online);
            Assert.Equal(createTeamDto.EditionId, teamDto.EditionId);
        }

        // Bad Weather: Test for creating a team with invalid data
        [Fact]
        public async Task Create_Team_Invalid_TeamCode()
        {
            var createTeamDto = new CreateTeamDto
            {
                Name = "Test Team",
                Code = null, // Invalid team code
                ContactName = "John Doe",
                ContactEmail = "john.doe@example.com",
                ContactPhone = "1234567890",
                Online = true,
                EditionId = 1
            };

            await Assert.ThrowsAsync<ValidationException>(() => _teamsService.CreateTeam(createTeamDto));
        }

        public new void Dispose()
        {
            _context.Dispose();
        }
    }
}
