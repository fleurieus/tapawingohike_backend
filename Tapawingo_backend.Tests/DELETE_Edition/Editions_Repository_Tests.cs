using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;

namespace Tapawingo_backend.Tests.DELETE_Edition
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

        //Good weather
        [Fact]
        public async Task DELETE_Edition()
        {
            var editions = _editionsRepository.GetAllEditions(1);
            Assert.Equal(2, editions.Count);

            await _editionsRepository.DeleteEditionAsync(2);

            var editionsWithOneRemovedEdition = _editionsRepository.GetAllEditions(1);
            Assert.Single(editionsWithOneRemovedEdition);
        }
    }
}
