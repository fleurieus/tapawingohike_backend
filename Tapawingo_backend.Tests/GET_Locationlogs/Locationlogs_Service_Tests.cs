using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Tapawingo_backend.Interface;

namespace Tapawingo_backend.Tests.GET_Locationlogs
{
    [Collection("Database collection")]
    public class Locationlogs_Service_Tests : TestBase
    {
        private readonly LocationlogsRepository _locationlogsRepository;
        private readonly LocationlogsService _locationlogsService;
        private readonly TeamRepository _teamRepository;
        private readonly DataContext _context;

        public Locationlogs_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            //Create a repository that works on the TEST DATABASE!!
            _locationlogsRepository = new LocationlogsRepository(_context);
            _teamRepository = new TeamRepository(_context);
            //Create a instance of the IMapper.
            _locationlogsService = new LocationlogsService(_locationlogsRepository, _teamRepository, new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            }).CreateMapper());
        }

        //Good Weather
        [Fact]
        public async Task Get_Locationlogs()
        {
            var locationlogs = await _locationlogsService.GetLocationlogsOnTeamAsync(1);

            Assert.NotNull(locationlogs);
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
