using AutoMapper;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.GET_Editions_By_Id
{
    [Collection("Database collection")]
    public class GET_EditionsById_service_Tests : TestBase
    {
        private readonly EditionsRepository _editionsRepository;
        private readonly OrganisationsRepository _organisationsRepository;
        private readonly EventsRepository _eventsRepository;
        private readonly EditionsService _editionsService;
        private readonly DataContext _context;

        public GET_EditionsById_service_Tests(DatabaseFixture fixture) : base(fixture)
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
        public void Get_Editions_Linked_To_Existing_EventID()
        {
            var edition = _editionsService.GetEditionById(1, 1);

            Assert.NotNull(edition);
            Assert.Equal("TestEdition1", edition.Name);
            Assert.Equal(1, edition.EventId);
        }

        //Bad weather
        [Fact]
        public void Get_Edition_By_Id_BadEventId()
        {
            Assert.Throws<ArgumentException>(() => _editionsService.GetEditionById(999, 1));
        }

        [Fact]
        public void Get_Edition_By_Id_BadEditionId()
        {
            Assert.Throws<ArgumentException>(() => _editionsService.GetEditionById(1, 999));
        }


    }
}