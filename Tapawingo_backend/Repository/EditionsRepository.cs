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

        public Edition CreateEdition(Edition newEdition)
        {
            _context.Editions.Add(newEdition);
            _context.SaveChanges();
            return newEdition;
        }
    }
}
