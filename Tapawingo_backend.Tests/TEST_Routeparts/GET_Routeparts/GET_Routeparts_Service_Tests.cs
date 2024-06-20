using AutoMapper;
using Tapawingo_backend.Data;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Tapawingo_backend.Tests.TEST_Routeparts.GET_Routeparts
{
    public class GETRouteparts_Service_Tests : TestBase
    {
        private readonly RoutepartsRepository _routepartsRepository;
        private readonly RoutepartsService _routepartsService;
        private readonly RoutesRepository _routesRepository;
        private readonly DestinationRepository _destinationRepository;
        private readonly FileRepository _fileRepository;
        private readonly DataContext _context;

        public GETRouteparts_Service_Tests(DatabaseFixture fixture) : base(fixture)
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
        public async Task Get_Routeparts_Of_Existing_Route()
        {
            var routeParts = await _routepartsService.GetRoutepartsAsync(3);

            Assert.NotNull(routeParts);
            Assert.Equal(2, routeParts.Count);
        }
        //

        //Bad weather
        [Fact]
        public async Task Get_Routeparts_Of_Nonexisting_Route()
        {
            var routeParts = await _routepartsService.GetRoutepartsAsync(999);

            Assert.Null(routeParts);
        }
        //
    }
}