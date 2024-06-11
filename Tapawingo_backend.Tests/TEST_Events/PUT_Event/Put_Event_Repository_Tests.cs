using Tapawingo_backend.Data;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Tests.TEST_Events.PUT_Event
{
    [Collection("Database collection")]
    public class Put_Event_Repository_Tests : TestBase
    {
        private readonly EventsRepository _eventsRepository;
        private readonly DataContext _context;
        
        public Put_Event_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _eventsRepository = new EventsRepository(_context);
        }
        
        //Good Weather
        [Fact]
        public void Put_Event()
        {
            var updatedEvent = new Event
            {
                Id = 1,
                Name = "TestEvent1Updated",
                OrganisationId = 1
            };
            _eventsRepository.UpdateEvent(1, updatedEvent);
            
            var twEvent = _eventsRepository.GetEventByIdAndOrganisationId(1, 1);
            Assert.NotNull(twEvent);
            Assert.Equal(1, twEvent.Id);
            Assert.Equal("TestEvent1Updated", twEvent.Name);
        }
        
        protected new void Dispose()
        {
            _context.Dispose();
        }

    }
}