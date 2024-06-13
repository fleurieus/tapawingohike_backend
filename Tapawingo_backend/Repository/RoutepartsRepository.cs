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

        public async Task<Routepart> CreateRoutePartAsync(Routepart newRoutepart)
        {
            _context.Routeparts.Add(newRoutepart);
            await _context.SaveChangesAsync();
            return newRoutepart;
        }

        public async Task<List<Routepart>> GetRoutepartsAsync(int route_id)
        {
            return await _context.Routeparts.Where(rp => rp.RouteId == route_id).ToListAsync();
        }
    }
}
