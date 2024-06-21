using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Tests.GET_Editions
{
    [Collection("Database collection")]
    public class GET_editions_repository_Tests : TestBase
    {
        private readonly EditionsRepository _editionsRepository;
        private readonly DataContext _context;

        public GET_editions_repository_Tests(DatabaseFixture fixture) : base(fixture)
        {
            _context = Context; //inject 'shared' context from TestBase
            _editionsRepository = new EditionsRepository(_context);
        }

        //Good weather
        [Fact]
        public async void Get_Edition_Linked_To_Existing_EventID()
        {
            var Event1Editions = await _editionsRepository.GetAllEditions(1);

            Assert.NotNull(Event1Editions);
            Assert.Equal(2, Event1Editions.Count);
            Assert.Equal(1, Event1Editions[0].Id);
            Assert.Equal(2, Event1Editions[1].Id);
        }


        //Bad weather
        [Fact]
        public async void Get_Edition_Linked_To_NonExisting_EventID()
        {
            var Event1Editions = await _editionsRepository.GetAllEditions(999);

            Assert.NotNull(Event1Editions);
            Assert.Empty(Event1Editions); //expect empty list
        }
    }
}
