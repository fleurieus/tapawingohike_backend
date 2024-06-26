using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.TEST_Routeparts.GET_Routepart
{
    public class GETRoutepart_Repository_Tests : TestBase
    {
        private readonly RoutepartsRepository _routepartsRepository;
        private readonly DataContext _context;

        public GETRoutepart_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _routepartsRepository = new RoutepartsRepository(_context);
        }

        //Goodweather
        [Fact]
        public async Task Get_Routepart_On_Route()
        {
            var routePart = await _routepartsRepository.GetRoutepartOnRouteAsync(3, 2);

            Assert.NotNull(routePart);
            Assert.Equal("Routepart2", routePart.Name);
            Assert.Equal("normal", routePart.RouteType);
            Assert.False(routePart.RoutepartZoom);
        }
        //
    }
}