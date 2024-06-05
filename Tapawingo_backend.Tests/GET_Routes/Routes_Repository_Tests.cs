using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Tests.GET_Routes
{
    [Collection("Database collection")]
    public class Routes_Repository_Tests : TestBase
    {
        private readonly RoutesRepository _routesRepository;
        private readonly DataContext _context;

        public Routes_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _routesRepository = new RoutesRepository(_context);
        }

        //Good Weather
        [Fact]
        public void Get_All_Routes()
        {
            var routes = _routesRepository.GetRoutes();

            Assert.NotNull(routes);
            Assert.Equal(2, routes.Count());
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
