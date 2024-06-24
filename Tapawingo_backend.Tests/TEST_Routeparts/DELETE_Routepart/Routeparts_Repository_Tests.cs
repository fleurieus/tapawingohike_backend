using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.TEST_Routeparts.DELETE_Routepart
{
    public class Routeparts_Repository_Tests : TestBase
    {
        private readonly RoutepartsRepository _routepartsRepository;
        private readonly DataContext _context;

        public Routeparts_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _routepartsRepository = new RoutepartsRepository(_context);
        }

        //Goodweather
        [Fact]
        public async Task Delete_Routepart()
        {
            Routepart existingroutepart = await _routepartsRepository.GetRoutepartOnRouteAsync(3, 2);

            var deletion = await _routepartsRepository.DeleteRoutepartOnRouteAsync(3, existingroutepart);

            Assert.True(deletion);

            //check that routepart is deletted
            var deletionComplete = await _routepartsRepository.RoutepartExists(2);

            Assert.False(deletionComplete);
        }
        //
    }
}