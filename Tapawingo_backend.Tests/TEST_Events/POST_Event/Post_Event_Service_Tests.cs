using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.TEST_Events.POST_Event
{
    [Collection("Database collection")]
    public class Post_Event_Service_Tests : TestBase
    {
        
        private readonly OrganisationsRepository _organisationsRepository;
        private readonly EventsRepository _eventsRepository;
        private readonly EventsService _eventsService;
        private readonly DataContext _context;
        
        public Post_Event_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _organisationsRepository = new OrganisationsRepository(_context);
            _eventsRepository = new EventsRepository(_context);
            _eventsService = new EventsService(_eventsRepository, new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            }).CreateMapper(), _organisationsRepository);
        }
        
        // Good Weather
        [Fact]
        public void Post_Event()
        {
            var newEvent = new CreateEventDto()
            {
                Name = "TestEvent3"
            };

            var result = _eventsService.CreateEvent(newEvent, 1) as ObjectResult;

            Assert.NotNull(result);
            var createdEvent = result.Value as Event;
            Assert.NotNull(createdEvent);
            Assert.Equal("TestEvent3", createdEvent.Name);
            Assert.Equal(1, createdEvent.OrganisationId);
        }
        
        // Bad Weather
        [Fact]
        public void Post_Event_With_Non_Existing_Organisation()
        {
            var newEvent = new CreateEventDto()
            {
                Name = "TestEvent3"
            };

            var result = _eventsService.CreateEvent(newEvent, 999) as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Organisation does not exist", result.Value);
        }

        [Fact]
        public void Post_Event_With_No_Name()
        {
            var newEvent = new CreateEventDto()
            {
                Name = ""
            };

            var result = _eventsService.CreateEvent(newEvent, 1) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Event name is required", result.Value);
        }
        
        [Fact]
        public void Post_Event_That_Already_Exists_For_Organisation()
        {
            _eventsRepository.CreateEvent(new Event { Name = "TestEvent3", OrganisationId = 1 });

            var newEvent = new CreateEventDto()
            {
                Name = "TestEvent3"
            };

            var result = _eventsService.CreateEvent(newEvent, 1) as ConflictObjectResult;

            Assert.NotNull(result);
            Assert.Equal(409, result.StatusCode);
            Assert.Equal("Event already exists for this organisation", result.Value);
        }

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}