using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Tests.PATCH_Edition
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
        public async Task PATCH_Edition()
        {
            Edition existingEdition = _editionsRepository.GetEditionById(3);
            UpdateEditionDto updateEditionDto = new UpdateEditionDto
            {
                Name = "ChangedName",
                StartDate = new DateTime(2021, 12, 12),
                EndDate = new DateTime(2021, 12, 13)
            };

            
            var updatedEdition = await _editionsRepository.UpdateEditionAsync(existingEdition, updateEditionDto);

            Assert.NotNull(updatedEdition);
            Assert.Equal("ChangedName", updatedEdition.Name);
        }
    }
}
