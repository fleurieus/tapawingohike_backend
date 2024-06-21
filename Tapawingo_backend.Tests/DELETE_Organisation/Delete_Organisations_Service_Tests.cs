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
using Microsoft.AspNetCore.Mvc;

namespace Tapawingo_backend.Tests.DELETE_Organisations
{
    [Collection("Database collection")]
    public class Delete_Organisations_Service_Tests : TestBase
    {
        private readonly OrganisationsRepository _organisationsRepository;
        private readonly OrganisationsService _organisationsService;
        private readonly DataContext _context;

        public Delete_Organisations_Service_Tests(DatabaseFixture fixture) : base(fixture)
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
        public async void Delete_Alonestanding_Organisation()
        {
            //check organisation still exists
            var organisation = await _organisationsRepository.OrganisationExists(4);
            Assert.True(organisation);

            //Delete this organisation
            var deleteOperationSuccessfull = await _organisationsService.DeleteOrganisationAsync(4);
            var expectedResult = new NoContentResult();
            Assert.Equal(deleteOperationSuccessfull.GetType(), expectedResult.GetType());

            //check again if organisation exists
            var organisation_check = await _organisationsRepository.OrganisationExists(4);
            Assert.False(organisation_check);
        }

        [Fact]
        public async void Delete_Alonestanding_Organisation_With_Cascading()
        {
            //check organisation still exists
            var organisation = await _organisationsRepository.OrganisationExists(5);
            Assert.True(organisation);
            //check event still exists
            var event_exists = _context.Events.Any(e => e.Id == 3);
            Assert.True(event_exists);

            //delete the organisation
            var deleteOperationSuccessfull = await _organisationsService.DeleteOrganisationAsync(5);
            var expectedResult = new NoContentResult();
            Assert.Equal(deleteOperationSuccessfull.GetType(), expectedResult.GetType());

            //check of both are deleted
            var organisation_check = await _organisationsRepository.OrganisationExists(5);
            Assert.False(organisation_check);
            var event_exists_check = _context.Events.Any(e => e.Id == 3);
            Assert.False(event_exists_check);
        }
        //

        //Bad weather
        [Fact]
        public async void Delete_Organisation_Non_Existing_Id()
        {
            var organisation = await _organisationsService.DeleteOrganisationAsync(999);
            var expectedResult = new NotFoundObjectResult(new { message = "Organisation not found" });
            Assert.Equal(organisation.GetType(), expectedResult.GetType());
        }

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
