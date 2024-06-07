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

namespace Tapawingo_backend.Tests.GET_Organisations_By_Id
{
    [Collection("Database collection")]
    public class Organisations_By_Id_Service_Tests : TestBase
    {
        private readonly OrganisationsRepository _organisationsRepository;
        private readonly OrganisationsService _organisationsService;
        private readonly DataContext _context;

        public Organisations_By_Id_Service_Tests(DatabaseFixture fixture) : base(fixture)
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
        public void Get_Existing_Organisation_By_Id()
        {
            var organisation = _organisationsService.GetOrganisationById(1);

            Assert.NotNull(organisation);
            Assert.Equal(1, organisation.Id);
            Assert.Equal("TestOrganisation1", organisation.Name);
        }
        // 


        //Bad Weather
        [Fact]
        public void Get_Non_Existing_ogranisation_By_Id()
        {
            var organisation = _organisationsService.GetOrganisationById(999);

            Assert.Null(organisation);
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
