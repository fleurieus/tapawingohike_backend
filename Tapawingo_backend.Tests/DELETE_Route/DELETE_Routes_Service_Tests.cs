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

namespace Tapawingo_backend.Tests.DELETE_Routes
{
    [Collection("Database collection")]
    public class DELETE_Routes_Service_Tests : TestBase
    {
        private readonly RoutesRepository _routesRepository;
        private readonly EditionsRepository _editionsRepository;
        private readonly RoutesService _routesService;
        private readonly DataContext _context;

        public DELETE_Routes_Service_Tests(DatabaseFixture fixture) : base(fixture)
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
        public async void Delete_Route()
        {
            var existingroute = await _routesRepository.RouteExists(5);

            Assert.True(existingroute);

            var deletion = await _routesService.DeleteRouteById(1, 5);

            Assert.True(deletion);

            //check that route is deletted

            var deletionComplete = await _routesRepository.RouteExists(5);

            Assert.False(deletionComplete);
        }
        //

        //BadWeather
        [Fact]
        public async void Check_Combination_Error()
        {
            var existingroute = await _routesRepository.RouteExists(1);

            Assert.True(existingroute);

            await Assert.ThrowsAsync<ArgumentException>(() => _routesService.DeleteRouteById(2, 1));
        }

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
