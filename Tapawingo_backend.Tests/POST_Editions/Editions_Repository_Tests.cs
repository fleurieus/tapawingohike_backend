using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Tests.POST_Editions
{
    [Collection("Database collection")]
    public class Editions_Repository_Tests : TestBase
    {
        private readonly EditionsRepository _editionsRepository;
        private readonly DataContext _context;

        public Editions_Repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _editionsRepository = new EditionsRepository(_context);
        }

        //Good Weather
        [Fact]
        public void Post_Edition()
        {
            var testName = "test";
            var testDate = DateTime.Now;

            Edition edition = new Edition();

            edition.Name = testName;
            edition.StartDate = testDate;
            edition.EndDate = testDate;
            edition.EventId = _context.Events.FirstOrDefault().Id;
            _editionsRepository.CreateEdition(edition);

            //There should only be one Edition with the testName
            var foundEdition = _context.Editions.FirstOrDefault(x => x.Name == testName);

            Assert.NotNull(foundEdition);
            Assert.Equal(testName, foundEdition.Name);
            Assert.Equal(testDate, foundEdition.StartDate);
            Assert.Equal(testDate, foundEdition.EndDate);
        }
        //

        protected new void Dispose()
        {
            _context.Dispose();
        }
    }
}
