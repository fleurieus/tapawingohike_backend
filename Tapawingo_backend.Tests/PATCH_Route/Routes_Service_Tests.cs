using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;
using AutoMapper;
using Tapawingo_backend.Helper;
using Microsoft.EntityFrameworkCore;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;

namespace Tapawingo_backend.Tests.PATCH_Route
{
    [Collection("Database collection")]
    public class Routes_Service_Tests : TestBase
    {
        private readonly RoutesRepository _routesRepository;
        private readonly EditionsRepository _editionsRepository;
        private readonly RoutesService _routesService;
        private readonly DataContext _context;

        public Routes_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            //Create a repository that works on the TEST DATABASE!!
            _routesRepository = new RoutesRepository( _context );
            _editionsRepository = new EditionsRepository(_context );
            //Create a instance of the IMapper.
            _routesService = new RoutesService(_routesRepository, _editionsRepository, new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            }).CreateMapper());
        }

        //Good Weather
        [Fact]
        public async Task PATCH_Route()
        {
            UpdateRouteDto updateRouteDto = new UpdateRouteDto
            {
                Name = "test99"
            };

            var result = await _routesService.UpdateRouteOnEditionAsync(1, 1, updateRouteDto);

            var routes = await _routesRepository.GetRoutesOnEditionAsync(1);
            var firstRoute = routes.First();

            Assert.NotNull(result);
            Assert.Equal("test99", firstRoute.Name);
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
