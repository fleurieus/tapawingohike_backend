using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.TEST_Routeparts.PUT_Routepart
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

            var result = await _routepartsRepository.UpdateRoutepartOnRouteAsync(routepart, updateRoutepartDto);

            Assert.NotNull(result);

            var updatedRoutepart = await _context.Routeparts.FirstOrDefaultAsync(r => r.Name == "test99");

            Assert.NotNull(updatedRoutepart);
            Assert.Single(routepart.Destinations);
            Assert.Single(routepart.Files);
        }
        //
    }
}