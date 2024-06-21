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
        public async void Post_Event()
        {
            var newEvent = new CreateEventDto()
            {
                Name = "TestEvent3"
            };

            var result = await _eventsService.CreateEvent(newEvent, 1) as ObjectResult;

            Assert.NotNull(result);
            var createdEvent = result.Value as EventDto;
            Assert.NotNull(createdEvent);
            Assert.Equal("TestEvent3", createdEvent.Name);
        }
        
        // Bad Weather
        [Fact]
        public async void Post_Event_With_Non_Existing_Organisation()
        {
            var newEvent = new CreateEventDto()
            {
                Name = "TestEvent3"
            };

            var result = await _eventsService.CreateEvent(newEvent, 999) as NotFoundObjectResult;
            var expectedResult = new NotFoundObjectResult(null);

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal(expectedResult.GetType(), result.GetType());
        }

        [Fact]
        public async void Post_Event_With_No_Name()
        {
            var newEvent = new CreateEventDto()
            {
                Name = ""
            };

            var result = await _eventsService.CreateEvent(newEvent, 1) as BadRequestObjectResult;
            var expectedResult = new BadRequestObjectResult(new { });

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(expectedResult.GetType(), result.GetType());
        }
        
        [Fact]
        public async void Post_Event_That_Already_Exists_For_Organisation()
        {
            await _eventsRepository.CreateEvent(new Event { Name = "TestEvent3", OrganisationId = 1 });

            var newEvent = new CreateEventDto()
            {
                Name = "TestEvent3"
            };

            var result = await _eventsService.CreateEvent(newEvent, 1) as ConflictObjectResult;
            var expectedResult = new ConflictObjectResult(new { });

            Assert.NotNull(result);
            Assert.Equal(409, result.StatusCode);
            Assert.Equal(expectedResult.GetType(), result.GetType());
        }

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}