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

namespace Tapawingo_backend.Tests.GET_Routes
{
    [Collection("Database collection")]
    public class Routes_Service_Tests : TestBase
    {
        private readonly RoutesRepository _routesRepository;
        private readonly RoutesService _routesService;
        private readonly DataContext _context;

        public Routes_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            //Create a repository that works on the TEST DATABASE!!
            _routesRepository = new RoutesRepository( _context );
            //Create a instance of the IMapper.
            _routesService = new RoutesService(_routesRepository, new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            }).CreateMapper());
        }

        //Good Weather
        [Fact]
        public void Get_All_Routes()
        {
            var routes = _routesService.GetRoutes();

            Assert.NotNull(routes);
            Assert.Equal(3, routes.Count());
            Assert.Equal(1, routes[0].Id);
            Assert.Equal(2, routes[1].Id);
            Assert.Equal("TestRoute1", routes[0].Name);
            Assert.Equal("TestRoute2", routes[1].Name);
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
