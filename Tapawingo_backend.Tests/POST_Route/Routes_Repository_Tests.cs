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

namespace Tapawingo_backend.Tests.POST_Route
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
        public async Task POST_Route()
        {
            var editionId = _context.Editions.First().Id;

            TWRoute route = new TWRoute
            {
                Name = "test99",
                EditionId = editionId
            };

            var result = await _routesRepository.CreateRouteOnEditionAsync(route);

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
