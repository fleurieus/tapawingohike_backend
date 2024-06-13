using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.POST_Routeparts
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
        public async Task Post_Routepart_No_Destinations_No_Files()
        {
            var routeId = _context.Routes.First().Id;

            Routepart routepart = new Routepart()
            {
                Name = "test100",
                RouteType = "Normal",
                RoutepartZoom = false,
                RoutepartFullscreen = true,
                Order = 1,
                Final = false,
                RouteId = routeId
            };

            var result = await _routepartsRepository.CreateRoutePartAsync(routepart);

            Assert.NotNull(result);

            var foundRoutepart = _context.Routeparts.FirstOrDefault(r => r.Name == "test100");

            Assert.NotNull(foundRoutepart);
            Assert.Equal("Normal", foundRoutepart.RouteType);
            Assert.False(foundRoutepart.RoutepartZoom);
            Assert.True(foundRoutepart.RoutepartFullscreen);
            Assert.False(foundRoutepart.Final);
            Assert.Equal(routeId, foundRoutepart.RouteId);
        }
        //
    }
}