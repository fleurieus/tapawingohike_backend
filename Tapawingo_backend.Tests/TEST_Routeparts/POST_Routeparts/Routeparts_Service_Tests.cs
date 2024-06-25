using AutoMapper;
using Tapawingo_backend.Data;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Tapawingo_backend.Tests.TEST_Routeparts.POST_Routeparts
{
    public class Routeparts_Service_Tests : TestBase
    {
        private readonly RoutepartsRepository _routepartsRepository;
        private readonly RoutepartsService _routepartsService;
        private readonly RoutesRepository _routesRepository;
        private readonly DestinationRepository _destinationRepository;
        private readonly FileRepository _fileRepository;
        private readonly DataContext _context;

        public Routeparts_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context;
            _routepartsRepository = new RoutepartsRepository(_context);
            _routesRepository = new RoutesRepository(_context);
            _destinationRepository = new DestinationRepository(_context);
            _fileRepository = new FileRepository(_context);

            _routepartsService = new RoutepartsService(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            }).CreateMapper(), _routepartsRepository, _routesRepository, _destinationRepository, _fileRepository);
        }

        //Goodweather
        [Fact]
        public async Task Post_Routepart_No_Destinations_No_Files()
        {
            var routeId = _context.Routes.First().Id;

            CreateRoutepartDto createRoutepartDto = new CreateRoutepartDto
            {
                Name = "test99",
                RouteType = "Normal",
                RoutepartZoom = false,
                RoutepartFullscreen = true,
                DestinationsJson = null,
                Files = null
            };

            var result = await _routepartsService.CreateRoutepartAsync(createRoutepartDto, routeId);

            Assert.NotNull(result);

            var foundRoutepart = await _context.Routeparts.FirstOrDefaultAsync(r => r.Name == "test99");

            Assert.NotNull(foundRoutepart);
            Assert.Equal("Normal", foundRoutepart.RouteType);
            Assert.False(foundRoutepart.RoutepartZoom);
            Assert.True(foundRoutepart.RoutepartFullscreen);
            Assert.True(foundRoutepart.Final);
            Assert.Equal(routeId, foundRoutepart.RouteId);
        }
        //

        //Goodweather
        [Fact]
        public async Task Post_Routepart()
        {
            var routeId = _context.Routes.First().Id;

            // Creating a byte array for the dummy file content
            var dummyFileContent = Encoding.UTF8.GetBytes("This is a dummy file");
            var fileName = "dummy.txt";

            // Creating a FormFile object with proper initialization
            var formFile = new FormFile(
                new MemoryStream(dummyFileContent),
                0,
                dummyFileContent.Length,
                "Data",
                fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };

            CreateRoutepartDto createRoutepartDto = new CreateRoutepartDto
            {
                Name = "test999",
                RouteType = "Normal",
                RoutepartZoom = false,
                RoutepartFullscreen = true,
                DestinationsJson = "[{\"Name\":\"testDestination\",\"Latitude\":1.0,\"Longitude\":1.0,\"Radius\":1,\"DestinationType\":\"Normal\",\"ConfirmByUser\":false,\"HideForUser\":false}]",
                Files = new List<IFormFile> { formFile }
            };

            var result = await _routepartsService.CreateRoutepartAsync(createRoutepartDto, routeId);

            Assert.NotNull(result);

            var routepart = await _context.Routeparts.FirstOrDefaultAsync(r => r.Name == "test999");
            var foundDestination = await _context.Destinations.FirstOrDefaultAsync(d => d.Name == "testDestination");
            var foundFile = await _context.Files.FirstOrDefaultAsync(f => f.File == "dummy.txt");


            Assert.NotNull(foundDestination);
            Assert.NotNull(routepart.Files);
            Assert.Equal("Normal", foundDestination.DestinationType);
            Assert.False(foundDestination.ConfirmByUser);

            Assert.Equal("dummy.txt", foundFile.File);
            Assert.Equal(Encoding.UTF8.GetBytes("This is a dummy file"), foundFile.Data);
        }
        //

        //Badweather
        [Fact]
        public async Task Post_Routepart_Faulty_RouteId()
        {
            CreateRoutepartDto createRoutepartDto = new CreateRoutepartDto
            {
                Name = "test99",
                RouteType = "Normal",
                RoutepartZoom = false,
                RoutepartFullscreen = true,
                DestinationsJson = null,
                Files = null
            };

            Assert.Null(await _routepartsService.CreateRoutepartAsync(createRoutepartDto, 99));
        }
        //
    }
}