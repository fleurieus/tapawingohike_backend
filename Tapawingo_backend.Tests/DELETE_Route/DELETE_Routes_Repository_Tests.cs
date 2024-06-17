using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Tests.DELETE_Routes
{
    [Collection("Database collection")]
    public class DELETE_Routes_Repository_Tests : TestBase
    {
        private readonly RoutesRepository _routesRepository;
        private readonly DataContext _context;

        public DELETE_Routes_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _routesRepository = new RoutesRepository(_context);
        }

        //Good Weather
        [Fact]
        public async void Delete_Route()
        {
            var existingroute = _routesRepository.RouteExists(4);

            Assert.True(existingroute);

            var deletion = await _routesRepository.DeleteRouteByIdAsync(4);

            Assert.True(deletion);

            //check that route is deletted

            var deletionComplete = _routesRepository.RouteExists(4);

            Assert.False(deletionComplete);
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
