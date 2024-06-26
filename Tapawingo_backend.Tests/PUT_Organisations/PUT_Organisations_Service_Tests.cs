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
using Tapawingo_backend.Dtos;

namespace Tapawingo_backend.Tests.PUT_Organisations
{
    [Collection("Database collection")]
    public class PUT_Organisations_Service_Tests : TestBase
    {
        private readonly OrganisationsRepository _organisationsRepository;
        private readonly OrganisationsService _organisationsService;
        private readonly DataContext _context;

        public PUT_Organisations_Service_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            //Create a repository that works on the TEST DATABASE!!
            _organisationsRepository = new OrganisationsRepository( _context );
            //Create a instance of the IMapper.
            _organisationsService = new OrganisationsService(_organisationsRepository, _context, new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            }).CreateMapper());
        }

        //Good Weather
        [Fact]
        public async void Update_A_Organisation_By_One_Element()
        {

            //before update
            var oldOrganisation = await _organisationsService.GetOrganisationById(3);

            Assert.NotNull(oldOrganisation);
            Assert.Equal("TestForUpdate", oldOrganisation.Name);

            //updating...
            var newOrganisationModel = new UpdateOrganisationDto
            {
                Name = "YetAnotherName"
            };

            await _organisationsService.UpdateOrganisation(3, newOrganisationModel);

            //after update
            var newOrganisation = await _organisationsService.GetOrganisationById(3);
            Assert.NotNull(newOrganisation);
            Assert.Equal("YetAnotherName", newOrganisation.Name);
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
