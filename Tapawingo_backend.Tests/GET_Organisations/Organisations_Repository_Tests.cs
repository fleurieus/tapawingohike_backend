using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Tests.GET_Organisations
{
    [Collection("Database collection")]
    public class Organisations_Repository_Tests : TestBase
    {
        private readonly OrganisationsRepository _organisationsRepository;
        private readonly DataContext _context;

        public Organisations_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _organisationsRepository = new OrganisationsRepository(_context);
        }

        //Good Weather
        [Fact]
        public void Get_All_Existing_Organisations()
        {
            var organisation = _organisationsRepository.GetAllOrganisations();

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
