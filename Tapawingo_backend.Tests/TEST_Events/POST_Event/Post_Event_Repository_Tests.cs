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
        public async void Post_Event()
        {
            var newEvent = new Event
            {
                Name = "TestEvent4",
                OrganisationId = 1
            };
            await _eventsRepository.CreateEvent(newEvent);
            
            var twEvent = await _eventsRepository.GetEventByIdAndOrganisationId(4, 1);
            Assert.NotNull(twEvent);
            Assert.Equal(4, twEvent.Id);
            Assert.Equal("TestEvent4", twEvent.Name);
        }
        //
        
        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}