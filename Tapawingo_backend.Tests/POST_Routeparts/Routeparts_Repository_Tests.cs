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
        public void Post_Routepart()
        {
            var testName = "test";
            var routeType = "Normal";
            var routepartZoom = false;
            var routepartFullscreen = false;
            var order = 1;
            var final = true;
            var routeId = _context.Routes.First().Id;

            Routepart routepart = new Routepart()
            {
                Name = testName,
                RouteType = routeType,
                RoutepartZoom = routepartZoom,
                RoutepartFullscreen = routepartFullscreen,
                Order = order,
                Final = final,
                RouteId = routeId
            };

            var result = _routepartsRepository.CreateRoutePartAsync(routepart);

            Assert.NotNull(result);

            var foundRoutepart = _context.Routeparts.FirstOrDefault(r => r.Name == testName);

            Assert.NotNull(foundRoutepart);
            Assert.Equal(routeType, foundRoutepart.RouteType);
            Assert.Equal(routepartZoom, foundRoutepart.RoutepartZoom);
            Assert.Equal(routepartFullscreen, foundRoutepart.RoutepartFullscreen);
            Assert.Equal(order, foundRoutepart.Order);
            Assert.Equal(final, foundRoutepart.Final);
            Assert.Equal(routeId, foundRoutepart.RouteId);
        }
        //
    }
}