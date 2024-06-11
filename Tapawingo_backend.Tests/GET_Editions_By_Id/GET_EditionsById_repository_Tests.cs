using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Tests.GET_Editions_By_Id
{
    [Collection("Database collection")]
    public class GET_EditionsById_repository_Tests : TestBase
    {
        private readonly EditionsRepository _editionsRepository;
        private readonly DataContext _context;

        public GET_EditionsById_repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _editionsRepository = new EditionsRepository(_context);
        }

        //Good weather
        [Fact]
        public void Get_Edition_By_Id()
        {
            var edition = _editionsRepository.GetEditionById(1, 1);

            Assert.NotNull(edition);
            Assert.Equal("TestEdition1", edition.Name);
            Assert.Equal(1, edition.EventId);
            Assert.Equal(1, edition.Id);
        }

        //Bad weather
        [Fact]
        public void Get_Edition_By_Id_BadEventId()
        {
            Assert.Throws<ArgumentException>(() => _editionsRepository.GetEditionById(999, 1));
        }

        [Fact]
        public void Get_Edition_By_Id_BadEditionId()
        {
            Assert.Throws<ArgumentException>(() => _editionsRepository.GetEditionById(1, 999));
        }
    }
}
