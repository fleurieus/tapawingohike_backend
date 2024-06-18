using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.GET_Locationlogs
{
    [Collection("Database collection")]
    public class Locationlogs_Repository_Tests : TestBase
    {
        private readonly LocationlogsRepository _locationlogsRepository;
        private readonly DataContext _context;

        public Locationlogs_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _locationlogsRepository = new LocationlogsRepository(_context);
        }

        //Good Weather
        [Fact]
        public async Task Get_Locationlogs()
        {
            var locationlogs = await _locationlogsRepository.GetLocationlogsOnTeamAsync(1);

            Assert.NotNull(locationlogs);
            Assert.Equal(2, locationlogs.Count());
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
