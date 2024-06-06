using Tapawingo_backend.Data;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Tests.TEST_Events.POST_Event
{
    [Collection("Database collection")]
    public class Post_Event_Repository_Tests : TestBase
    {
        
        private readonly EventsRepository _eventsRepository;
        private readonly DataContext _context;
        
        public Post_Event_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _eventsRepository = new EventsRepository(_context);
        }
        
        //Good Weather
        [Fact]
        public void Post_Event()
        {
            var newEvent = new Event
            {
                Name = "TestEvent3",
                OrganisationId = 1
            };
            _eventsRepository.CreateEvent(newEvent);
            
            var twEvent = _eventsRepository.GetEventByIdAndOrganisationId(3, 1);
            Assert.NotNull(twEvent);
            Assert.Equal(3, twEvent.Id);
            Assert.Equal("TestEvent3", twEvent.Name);
        }
        //
        
        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}