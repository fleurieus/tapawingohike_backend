using Microsoft.EntityFrameworkCore;
using Tapawingo_backend.Data;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public class RoutepartsRepository : IRoutepartsRepository
    {
        private readonly DataContext _context;

        public RoutepartsRepository(DataContext context) 
        {
            _context = context;
        }

        public Routepart CreateRoutePart(Routepart newRoutepart)
        {
            _context.Routeparts.Add(newRoutepart);
            _context.SaveChanges();
            return newRoutepart;
        }
    }
}
