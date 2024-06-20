using AutoMapper;
using Tapawingo_backend.Data;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Tapawingo_backend.Tests.TEST_Routeparts.PUT_Routepart
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
        public async Task Put_Routepart()
        {
            var routepart = await _routepartsRepository.GetRoutepartOnRouteAsync(3, 1);
            Assert.Equal(2, routepart.Destinations.Count);
            
            // Creating a byte array for the dummy file content
            var dummyFileContent = Encoding.UTF8.GetBytes("BLOB");
            var fileName = "test.png";

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

            UpdateRoutepartDto updateRoutepartDto = new UpdateRoutepartDto
            {
                Name = "test99",
                RouteType = "Normal",
                RoutepartZoom = false,
                RoutepartFullscreen = true,
                Final = false,
                DestinationsJson = "[{\"Name\":\"testDestination\",\"Latitude\":1.0,\"Longitude\":1.0,\"Radius\":1,\"DestinationType\":\"Normal\",\"ConfirmByUser\":false,\"HideForUser\":false}]",
                Files = new List<IFormFile> { formFile }
            };

            var result = await _routepartsService.UpdateRoutepartOnRouteAsync(3, 1, updateRoutepartDto);

            Assert.NotNull(result);

            var updatedRoutepart = await _context.Routeparts.FirstOrDefaultAsync(r => r.Name == "test99");

            Assert.NotNull(updatedRoutepart);
            Assert.Single(routepart.Destinations);
            Assert.Single(routepart.Files);
        }
        //
    }
}