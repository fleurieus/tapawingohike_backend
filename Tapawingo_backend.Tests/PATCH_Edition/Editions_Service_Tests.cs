using AutoMapper;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Helper;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.PATCH_Edition
{
    [Collection("Database collection")]
    public class Editions_Service_Tests : TestBase
    {
        private readonly EditionsRepository _editionsRepository;
        private readonly OrganisationsRepository _organisationsRepository;
        private readonly EventsRepository _eventsRepository;
        private readonly EditionsService _editionsService;
        private readonly DataContext _context;

        public Editions_Service_Tests(DatabaseFixture fixture) : base(fixture)
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
        public async Task PATCH_Edition()
        {
            UpdateEditionDto updateEditionDto = new UpdateEditionDto
            {
                Name = "ChangedName",
                StartDate = new DateTime(2021, 12, 12),
                EndDate = new DateTime(2021, 12, 13)
            };

            var edition = await _editionsService.UpdateEditionAsync(2, 3, updateEditionDto);

            Assert.NotNull(edition);
            Assert.Equal("ChangedName", edition.Name);
        }
    }
}