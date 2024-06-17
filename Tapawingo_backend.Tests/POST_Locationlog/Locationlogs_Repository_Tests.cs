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

namespace Tapawingo_backend.Tests.POST_Locationlog
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
        public async Task Post_Locationlog()
        {       
            Locationlog locationlog = new Locationlog
            {
                Latitude = 500,
                Longitude = 1000,
                Timestamp = DateTime.Now,
                TeamId = 1
            };

            var savedLocationlog = await _locationlogsRepository.CreateLocationlogOnTeamAsync(locationlog);

            Assert.NotNull(savedLocationlog);
            Assert.Equal(500, savedLocationlog.Latitude);
            Assert.Equal(1000, savedLocationlog.Longitude);
            Assert.Equal(1, savedLocationlog.TeamId);
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
