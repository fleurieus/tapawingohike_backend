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

namespace Tapawingo_backend.Tests.POST_Route
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
        public async Task POST_Route()
        {
            var editionId = _context.Editions.First().Id;

            CreateRouteDto createRouteDto = new CreateRouteDto
            {
                Name = "test99"
            };

            var result = await _routesService.CreateRouteOnEditionAsync(editionId, createRouteDto);

            Assert.NotNull(result);

            var foundRoute = await _context.Routes.FirstOrDefaultAsync(r => r.Name == "test99");

            Assert.NotNull(foundRoute);
            Assert.Equal("test99", foundRoute.Name);
            Assert.Equal(editionId, foundRoute.EditionId);
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
