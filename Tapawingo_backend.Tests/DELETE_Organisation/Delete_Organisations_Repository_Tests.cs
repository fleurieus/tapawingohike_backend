using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Tests.DELETE_Organisations
{
    [Collection("Database collection")]
    public class Delete_Organisations_Repository_Tests : TestBase
    {
        private readonly OrganisationsRepository _organisationsRepository;
        private readonly DataContext _context;

        public Delete_Organisations_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _organisationsRepository = new OrganisationsRepository(_context);
        }

        //Good Weather
        [Fact]
        public async void Delete_Alonestanding_Organisation()
        {
            //check organisation still exists
            var organisation = await _organisationsRepository.OrganisationExists(4);
            Assert.True(organisation);

            //Delete this organisation
            var deleteOperationSuccessfull = await _organisationsRepository.DeleteOrganisationAsync(4);
            Assert.True(deleteOperationSuccessfull);

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
            var deleteOperationSuccessfull = await _organisationsRepository.DeleteOrganisationAsync(5);
            Assert.True(deleteOperationSuccessfull);

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
            var organisation = await _organisationsRepository.DeleteOrganisationAsync(999);
            Assert.False(organisation); // No exception since this is covered in service.
        }

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
