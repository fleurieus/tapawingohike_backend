using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;
using AutoMapper;
using Tapawingo_backend.Helper;

namespace Tapawingo_backend.Tests.GET_Organisations
{
    [Collection("Database collection")]
    public class Organisations_Service_Tests : TestBase
    {
        private readonly OrganisationsRepository _organisationsRepository;
        private readonly OrganisationsService _organisationsService;
        private readonly DataContext _context;

        public Organisations_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            //Create a repository that works on the TEST DATABASE!!
            _organisationsRepository = new OrganisationsRepository( _context );
            //Create a instance of the IMapper.
            _organisationsService = new OrganisationsService(_organisationsRepository, new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            }).CreateMapper());
        }

        //Good Weather
        [Fact]
        public void Get_All_Existing_Organisations()
        {
            var organisation = _organisationsService.GetOrganisations();

            Assert.NotNull(organisation);
            Assert.Equal(5, organisation.Count());
            Assert.Equal(1, organisation[0].Id);
            Assert.Equal(2, organisation[1].Id);
            Assert.Equal("TestOrganisation1", organisation[0].Name);
            Assert.Equal("TestOrganisation2", organisation[1].Name);
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
