using Microsoft.EntityFrameworkCore;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public class EditionsRepository : IEditionsRepository
    {
        private readonly DataContext _context;

        public EditionsRepository(DataContext context)
        {
            _context = context;
        }

        public Edition GetEditionById(int editionId)
        {
            return _context.Editions.FirstOrDefault(e => e.Id == editionId);
        }

        public List<Edition> GetAllEditions(int eventId)
        {
            return _context.Editions.Where(e => e.EventId == eventId).ToList();
        }
        public async Task<Edition> CreateEditionOnEventAsync(Edition edition)
        {
            _context.Editions.Add(edition);
            await _context.SaveChangesAsync();
            return edition;
        }

        public bool EditionExists(int editionId)
        {
            bool editionExists = _context.Editions.Any(u => u.Id == editionId);
            return editionExists;
        }

        public async Task<Edition> UpdateEditionAsync(Edition existingEdition, UpdateEditionDto updateEditionDto)
        {
            if (updateEditionDto.Name != null)
            {
                existingEdition.Name = updateEditionDto.Name;
                _context.Editions.Update(existingEdition);
            }

            if (updateEditionDto.StartDate != null)
            {
                existingEdition.StartDate = (DateTime)updateEditionDto.StartDate;
                _context.Editions.Update(existingEdition);
            }

            if (updateEditionDto.EndDate != null)
            {
                existingEdition.EndDate = (DateTime)updateEditionDto.EndDate;
                _context.Editions.Update(existingEdition);
            }

            await _context.SaveChangesAsync();

            return existingEdition;
        }

        public async Task<bool> DeleteEditionAsync(int editionId)
        {
            try
            {
                _context.Editions.Remove(GetEditionById(editionId));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> EventExistsOnEdition(int eventId, int editionId)
        {
            var edition = await _context.Editions.FirstOrDefaultAsync(e => e.Id == editionId);
            if (edition == null) return false;
            return edition.EventId == eventId;
        }
    }
}
