using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Data;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.EventTests.GET_Events
{

    [Collection("Database collection")]
    public class Events_By_Id_Service_Tests : TestBase
    {
        private readonly OrganisationsRepository _organisationsRepository;
        private readonly EventsRepository _eventsRepository;
        private readonly EventsService _eventsService;
        private readonly DataContext _context;
        
        public Events_By_Id_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _organisationsRepository = new OrganisationsRepository(_context);
            _eventsRepository = new EventsRepository(_context);
            _eventsService = new EventsService(_eventsRepository, new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            }).CreateMapper(), _organisationsRepository);
        }
        
        //Good Weather
        [Fact]
        public void Get_Event_By_Id_And_OrganisationId()
        {
            var twEvent = _eventsService.GetEventByIdAndOrganisationId(1, 1);
            var expectedResult = new OkObjectResult(null);

            Assert.Equal(expectedResult.GetType(), twEvent.GetType());
        }
        //
        
        //Bad Weather
        [Fact]
        public void Get_Non_Existing_Event_By_Id_And_OrganisationId()
        {
            var twEvent = _eventsService.GetEventByIdAndOrganisationId(999, 999);
            var expectedResult = new NotFoundObjectResult(null);

            Assert.Equal(expectedResult.GetType(), twEvent.GetType());
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}