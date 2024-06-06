using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Dtos;

namespace Tapawingo_backend.Tests.POST_Editions
{
    [Collection("Database collection")]
    public class Editions_Service_Tests : TestBase
    {
        private readonly EditionsRepository _editionsRepository;
        private readonly EditionsService _editionsService;
        private readonly DataContext _context;
        private readonly OrganisationsRepository _organisationsRepository;
        private readonly EventsRepository _eventsRepository;

        public Editions_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            //Create a repository that works on the TEST DATABASE!!
            _editionsRepository = new EditionsRepository( _context );
            _organisationsRepository = new OrganisationsRepository( _context );
            _eventsRepository = new EventsRepository( _context );
            //Create a instance of the IMapper.
            _editionsService = new EditionsService(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            }).CreateMapper(), _editionsRepository, _organisationsRepository, _eventsRepository);
        }

        //Good Weather
        [Fact]
        public void Post_Edition()
        {
            // Test data to test against.
            var testName = "test";
            var testDate = DateTime.Now;
            var eventId = _context.Events.FirstOrDefault().Id;
            var organisationId = _context.Organisations.FirstOrDefault().Id;

            CreateEditionDto createEditionDto = new CreateEditionDto { 
                Name = testName, StartDate = testDate, EndDate = testDate 
            };

            // Pak default Organisation en Event
            var edition = _editionsService.CreateEdition(createEditionDto, eventId, organisationId);

            Assert.NotNull(edition);

            //There should only be one Edition with the testName
            var foundEdition = _context.Editions.FirstOrDefault(x => x.Name == testName);

            Assert.NotNull(foundEdition);
            Assert.Equal(testName, foundEdition.Name);
            Assert.Equal(testDate, foundEdition.StartDate);
            Assert.Equal(testDate, foundEdition.EndDate);
        }
        //

        //Bad Weather
        [Fact]
        public void Post_Edition_Faulty_OrganisationId()
        {
            // Test data to test against.
            var testName = "testName";
            var testDate = DateTime.Now;

            CreateEditionDto createEditionDto = new CreateEditionDto
            {
                Name = testName,
                StartDate = testDate,
                EndDate = testDate
            };

            // Pak default Organisation en Event
            Assert.Throws<ArgumentException>(() => _editionsService.CreateEdition(createEditionDto, 0, 1));
        }
        //

        //Bad Weather
        [Fact]
        public void Post_Edition_Faulty_EventId()
        {
            // Test data to test against.
            var testName = "testName";
            var testDate = DateTime.Now;

            

            CreateEditionDto createEditionDto = new CreateEditionDto
            {
                Name = testName,
                StartDate = testDate,
                EndDate = testDate
            };

            // Pak default Organisation en Event
            Assert.Throws<ArgumentException>(() => _editionsService.CreateEdition(createEditionDto, 0, 1));
        }
        //

        //Bad Weather
        [Fact]
        public void Post_Edition_Mismatched_Id()
        {
            // Test data to test against.
            var testName = "testName";
            var testDate = DateTime.Now;

            CreateEditionDto createEditionDto = new CreateEditionDto
            {
                Name = testName,
                StartDate = testDate,
                EndDate = testDate
            };

            // Pak default Organisation en Event
            Assert.Throws<InvalidOperationException>(() => _editionsService.CreateEdition(createEditionDto, 2, 1));
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
