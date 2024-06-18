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

        public async Task<List<Routepart>> GetRoutepartsAsync(int route_id)
        {
            return await _context.Routeparts.Where(rp => rp.RouteId == route_id).ToListAsync();
        }

        public async Task<Routepart> GetRoutepartOnRouteAsync(int routeId, int routepartId)
        {
            return await _context.Routeparts
                .Include(rp => rp.Destinations)
                .Include(d => d.Files)
                .FirstOrDefaultAsync(rp => rp.RouteId == routeId && rp.Id == routepartId);
        }

        public bool RoutepartExists(int routepartId)
        {
            return _context.Routeparts.Any(rp => rp.Id == routepartId);
        }

        public async Task<Routepart> CreateRoutePartAsync(Routepart newRoutepart)
        {
            _context.Routeparts.Add(newRoutepart);
            await _context.SaveChangesAsync();
            return newRoutepart;
        }
    }
}
