using Tapawingo_backend.Data;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Tests.TEST_Events.GET_Events;

namespace Tapawingo_backend.Tests.TEST_Events.DELETE_Event
{
    [Collection("Database collection")]
    public class Events_Delete_Repository : TestBase
    {
        private readonly EventsRepository _eventsRepository;
        private readonly DataContext _context;
        
        public Events_Delete_Repository(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _eventsRepository = new EventsRepository(_context);
        }
        
        //Good Weather
        [Fact]
        public async void Delete_Existing_Event()
        { 
            await _eventsRepository.DeleteEvent(1);
            var twEvent = await _eventsRepository.GetEventById(1);
            Assert.Null(twEvent);
        }
        
        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}