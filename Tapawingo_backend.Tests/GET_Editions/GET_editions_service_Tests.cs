using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.GET_Editions
{
    [Collection("Database collection")]
    public class GET_editions_service_Tests : TestBase
    {
        private readonly EditionsRepository _editionsRepository;
        private readonly OrganisationsRepository _organisationsRepository;
        private readonly EventsRepository _eventsRepository;
        private readonly EditionsService _editionsService;
        private readonly DataContext _context;

        public GET_editions_service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _editionsRepository = new EditionsRepository(_context);
            _organisationsRepository = new OrganisationsRepository(_context);
            _eventsRepository = new EventsRepository(_context);
            _editionsService = new EditionsService(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            }).CreateMapper(), _editionsRepository, _organisationsRepository, _eventsRepository);
        }

        //Good weather
        [Fact]
        public async void Get_Editions_Linked_To_Existing_EventID()
        {
            var Event1Editions = await _editionsService.GetAllEditions(1);

            Assert.NotNull(Event1Editions);
            Assert.Equal(2, Event1Editions.Count);
        }


        //Bad weather
        public async void Get_Editions_Linked_To_NonExisting_EventID()
        {
            var Event1Editions = await _editionsService.GetAllEditions(999);

            Assert.NotNull(Event1Editions);
            Assert.Empty(Event1Editions); //expect empty list
        }
    }
}
