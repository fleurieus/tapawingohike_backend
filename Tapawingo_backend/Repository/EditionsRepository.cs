using Microsoft.EntityFrameworkCore;
using Tapawingo_backend.Data;
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

        public Edition GetEditionById(int eventId, int editionId)
        {
            var edition = GetAllEditions(eventId).FirstOrDefault(e => e.Id == editionId);
            if (edition == null) throw new ArgumentException("Edition not found.");
            return edition;
        }

        public List<Edition> GetAllEditions(int eventId)
        {
            return _context.Editions.Where(e => e.EventId == eventId).ToList();
        }
        public Edition CreateEdition(Edition newEdition)
        {
            _context.Editions.Add(newEdition);
            _context.SaveChanges();
            return newEdition;
        }
    }
}
