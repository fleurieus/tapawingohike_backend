using Tapawingo_backend.Data;
using Tapawingo_backend.Repository;
using Xunit.Abstractions;

namespace Tapawingo_backend.Tests.EventTests.GET_Events
{
    [Collection("Database collection")]
    public class Events_By_Id_Repository_Tests : TestBase
    {
        private readonly EventsRepository _eventsRepository;
        private readonly DataContext _context;
        
        public Events_By_Id_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _eventsRepository = new EventsRepository(_context);
        }
        
        //Good Weather
        [Fact]
        public void Get_Existing_Event_By_Id_And_Organisation_Id()
        { 
            var twEvent = _eventsRepository.GetEventByIdAndOrganisationId(1, 1);
            Assert.NotNull(twEvent);
            Assert.Equal(1, twEvent.Id);
            Assert.Equal("TestEvent1", twEvent.Name);
        }
        //


        //Bad Weather
        [Fact]
        public void Get_Non_Existing_Organisation_By_Id()
        {
            var twEvent = _eventsRepository.GetEventByIdAndOrganisationId(999, 999);

            Assert.Null(twEvent);
        }
        //
        
        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}