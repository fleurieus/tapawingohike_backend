using AutoMapper;
using Tapawingo_backend.Data;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Tapawingo_backend.Tests.GET_Routepart
{
    public class GETRoutepart_Service_Tests : TestBase
    {
        private readonly RoutepartsRepository _routepartsRepository;
        private readonly RoutepartsService _routepartsService;
        private readonly RoutesRepository _routesRepository;
        private readonly DestinationRepository _destinationRepository;
        private readonly FileRepository _fileRepository;
        private readonly DataContext _context;

        public GETRoutepart_Service_Tests(DatabaseFixture fixture) : base(fixture)
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
        public async Task Get_Routepart_On_Route()
        {
            var routePart = await _routepartsService.GetRoutepartOnRouteAsync(3,1);

            Assert.NotNull(routePart);
            Assert.Equal("Routepart1", routePart.Name);
            Assert.Equal("normal", routePart.RouteType);
            Assert.False(routePart.RoutepartZoom);
        }
        //
    }
}