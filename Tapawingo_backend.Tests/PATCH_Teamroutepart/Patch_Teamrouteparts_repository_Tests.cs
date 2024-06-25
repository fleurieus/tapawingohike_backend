using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.PATCH_Teamroutepart
{
    [Collection("Database collection")]
    public class Patch_Teamrouteparts_repository_Tests : TestBase
    {
        private readonly TeamroutepartsRepository _teamroutepartsRepository;
        private readonly DataContext _context;

        public Patch_Teamrouteparts_repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _teamroutepartsRepository = new TeamroutepartsRepository(_context);
        }

        [Fact]
        public async void Test_patch_teamroutepart()
        {
            //not pretty but needed since there is no getter functions for team route parts
            var teamRoutepart = _context.TeamRouteparts.FirstOrDefault(trp => trp.TeamId == 1 && trp.RoutepartId == 1);
        
            Assert.NotNull(teamRoutepart);
            Assert.Equal(1, teamRoutepart.TeamId);
            Assert.Equal(1, teamRoutepart.RoutepartId);
            Assert.False(teamRoutepart.IsFinished);
            Assert.Equal(DateTime.MinValue, teamRoutepart.CompletedTime);

            //update now
            await _teamroutepartsRepository.UpdateTeamRoutePart(1, 1, true);

            var updatedTeamRoutepart = _context.TeamRouteparts.FirstOrDefault(trp => trp.TeamId == 1 && trp.RoutepartId == 1);

            Assert.NotNull(updatedTeamRoutepart);
            Assert.Equal(1, updatedTeamRoutepart.TeamId);
            Assert.Equal(1, updatedTeamRoutepart.RoutepartId);
            Assert.True(updatedTeamRoutepart.IsFinished);
            //we cant check date for certain to achieve 100% testable values. What if test is runned at 00:00h?
            //Assert.Equal(DateTime.MinValue, teamRoutepart.CompletedTime);

            //and reset now
            await _teamroutepartsRepository.UpdateTeamRoutePart(1, 1, false);
            var resetTeamroutepart = _context.TeamRouteparts.FirstOrDefault(trp => trp.TeamId == 1 && trp.RoutepartId == 1);

            Assert.NotNull(resetTeamroutepart);
            Assert.Equal(1, resetTeamroutepart.TeamId);
            Assert.Equal(1, resetTeamroutepart.RoutepartId);
            Assert.False(resetTeamroutepart.IsFinished);
            Assert.NotEqual(DateTime.MinValue, resetTeamroutepart.CompletedTime);
        }
    }
}
