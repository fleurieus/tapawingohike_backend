using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.PATCH_Route
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
        public async Task PATCH_Route()
        {
            var routes = await _routesRepository.GetRoutesOnEditionAsync(1);
            var firstRoute = routes.First();

            UpdateRouteDto updateRouteDto = new UpdateRouteDto
            {
                Name = "test99"
            };

            var result = await _routesRepository.UpdateRouteOnEditionAsync(firstRoute, updateRouteDto);

            Assert.NotNull(result);
            Assert.Equal("test99", result.Name);
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
