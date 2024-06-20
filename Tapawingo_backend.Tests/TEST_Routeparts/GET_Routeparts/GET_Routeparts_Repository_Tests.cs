using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.TEST_Routeparts.GET_Routeparts
{
    public class GETRouteparts_Repository_Tests : TestBase
    {
        private readonly RoutepartsRepository _routepartsRepository;
        private readonly DataContext _context;

        public GETRouteparts_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _routepartsRepository = new RoutepartsRepository(_context);
        }

        //Goodweather
        [Fact]
        public async Task Get_Routeparts_Of_Existing_Route()
        {
            var routeParts = await _routepartsRepository.GetRoutepartsAsync(3);

            Assert.NotNull(routeParts);
            Assert.Equal(2, routeParts.Count);
        }
        //

        //Bad weather
        [Fact]
        public async Task Get_Routeparts_Of_Nonexisting_Route()
        {
            var routeParts = await _routepartsRepository.GetRoutepartsAsync(999);

            Assert.NotNull(routeParts);
            Assert.Empty(routeParts);
        }
        //
    }
}