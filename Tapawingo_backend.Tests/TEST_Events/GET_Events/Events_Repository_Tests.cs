using Tapawingo_backend.Data;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Tests.TEST_Events.GET_Events
{
    [Collection("Database collection")]
    public class Events_Repository_Tests : TestBase
    {
        private readonly EventsRepository _eventsRepository;
        private readonly DataContext _context;
        
        public Events_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _eventsRepository = new EventsRepository(_context);
        }
        
        //Good Weather
        [Fact]
        public async void Get_Existing_Events_By_Organisation_Id()
        { 
            var twEvents = await _eventsRepository.GetEventsByOrganisationId(1, null);
            Assert.NotNull(twEvents);
            Assert.Equal(2, twEvents.Count);
            Assert.Equal(1, twEvents[0].Id);
            Assert.Equal(2, twEvents[1].Id);
            Assert.Equal("TestEvent1", twEvents[0].Name);
            Assert.Equal("TestEvent2", twEvents[1].Name);
        }
        //
        
        //Bad Weather
        [Fact]
        public async void Get_Non_Existing_Events_By_Organisation_Id()
        {
            var twEvents = await _eventsRepository.GetEventsByOrganisationId(999, null);

            Assert.Empty(twEvents);
        }
        
        protected new void Dispose()
        {
            _context.Dispose();
        }
        

    }
}