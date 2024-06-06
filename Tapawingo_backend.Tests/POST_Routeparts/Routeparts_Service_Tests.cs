using AutoMapper;
using Tapawingo_backend.Data;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Dtos;

namespace Tapawingo_backend.Tests.POST_Routeparts
{
    public class Routeparts_Service_Tests : TestBase
    {
        private readonly RoutepartsRepository _routepartsRepository;
        private readonly RoutepartsService _routepartsService;
        private readonly RoutesRepository _routesRepository;
        private readonly DataContext _context;

        public Routeparts_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            //Create a repository that works on the TEST DATABASE!!
            _routepartsRepository = new RoutepartsRepository(_context);
            _routesRepository = new RoutesRepository(_context);
            //Create a instance of the IMapper.
            _routepartsService = new RoutepartsService(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            }).CreateMapper(), _routepartsRepository, _routesRepository);
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

            CreateRoutepartDto createRoutepartDto = new CreateRoutepartDto
            {
                Name = testName,
                RouteType = routeType,
                RoutepartZoom = routepartZoom,
                RoutepartFullscreen = routepartFullscreen,
                Order = order,
                Final = final,
            };

            var result = _routepartsService.CreateRoutepart(createRoutepartDto, routeId);

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

        //Badweather
        [Fact]
        public void Post_Routepart_Faulty_RouteId()
        {
            var testName = "test";
            var routeType = "Normal";
            var routepartZoom = false;
            var routepartFullscreen = false;
            var order = 1;
            var final = true;

            CreateRoutepartDto createRoutepartDto = new CreateRoutepartDto
            {
                Name = testName,
                RouteType = routeType,
                RoutepartZoom = routepartZoom,
                RoutepartFullscreen = routepartFullscreen,
                Order = order,
                Final = final,
            };

            Assert.Throws<InvalidOperationException>(() => _routepartsService.CreateRoutepart(createRoutepartDto, 0));
        }
        //
    }
}