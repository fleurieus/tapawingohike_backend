using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.TEST_Events.PUT_Event
{
    [Collection("Database collection")]
    public class Put_Event_Service_tests : TestBase
    {
        private readonly OrganisationsRepository _organisationsRepository;
        private readonly EventsRepository _eventsRepository;
        private readonly EventsService _eventsService;
        private readonly DataContext _context;

        public Put_Event_Service_tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _organisationsRepository = new OrganisationsRepository(_context);
            _eventsRepository = new EventsRepository(_context);
            _eventsService = new EventsService(_eventsRepository,
                new MapperConfiguration(cfg => { cfg.AddProfile(new MappingProfiles()); }).CreateMapper(),
                _organisationsRepository);
        }

        // Good Weather
        [Fact]
        public void Put_Event()
        {
            var updatedEvent = new CreateEventDto()
            {
                Name = "TestEvent1Updated"
            };

            var result = _eventsService.UpdateEvent(updatedEvent, 1, 1) as ObjectResult;

            Assert.NotNull(result);

            var twEvent = result.Value as Event;
            Assert.NotNull(twEvent);
            Assert.Equal(1, twEvent.Id);
            Assert.Equal("TestEvent1Updated", twEvent.Name);
        }

        // Bad Weather
        [Fact]
        public void Put_Event_With_Non_Existing_Event()
        {
            var updatedEvent = new CreateEventDto()
            {
                Name = "TestEvent1Updated"
            };

            var result = _eventsService.UpdateEvent(updatedEvent, 1, 999) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Event does not exist", result.Value);
        }

        [Fact]
        public void Put_Event_With_No_Name()
        {
            var updatedEvent = new CreateEventDto()
            {
                Name = ""
            };

            var result = _eventsService.UpdateEvent(updatedEvent, 1, 1) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Event name is required", result.Value);
        }

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}