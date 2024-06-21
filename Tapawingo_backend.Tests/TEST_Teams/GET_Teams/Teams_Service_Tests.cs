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

namespace Tapawingo_backend.Tests.TEST_Teams.GET_Teams
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
        public async void Get_Teams()
        {
            var teams = await _teamsService.GetTeamsOnEdition(1);

            Assert.NotNull(teams);
            Assert.Equal(2, teams.Count());
        }

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
