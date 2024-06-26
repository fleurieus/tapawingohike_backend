using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Controllers;
using Tapawingo_backend.Data;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.TEST_Events.GET_Events
{
    [Collection("Database collection")]
    public class Events_Service_Tests : TestBase
    {
        private readonly OrganisationsRepository _organisationsRepository;
        private readonly EventsRepository _eventsRepository;
        private readonly EventsService _eventsService;
        private readonly DataContext _context;
        
        public Events_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _eventsRepository = new EventsRepository(_context);
            _organisationsRepository = new OrganisationsRepository(_context);
            _eventsService = new EventsService(_eventsRepository, new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            }).CreateMapper(), _organisationsRepository);
        }
        
        //Good Weather
        [Fact]
        public async void Get_Existing_Events_By_Organisation_Id()
        { 
            var twEvents = await _eventsService.GetEventsByOrganisationId(1, "SuperAdmin");
            var expectedResult = new OkObjectResult(null);
            Assert.Equal(expectedResult.GetType(), twEvents.GetType());
        }
        //
        
        //Bad Weather
        [Fact]
        public async void Get_Non_Existing_Events_By_Organisation_Id()
        {
            var twEvents = await _eventsService.GetEventsByOrganisationId(999, null);
            var expectedResult = new NotFoundObjectResult(null);
            Assert.Equal(expectedResult.GetType(), twEvents.GetType());
        }
        
        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}