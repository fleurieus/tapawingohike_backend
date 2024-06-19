using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tapawingo_backend.Data;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Repository
{
    public class RoutesRepository : IRoutesRepository
    {
        private readonly DataContext _context;

        public RoutesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<TWRoute>> GetRoutesOnEditionAsync(int editionId)
        {
            return await _context.Routes.Where(route => route.EditionId == editionId).ToListAsync();
        }

        public async Task<TWRoute> GetRouteByIdAsync(int id)
        {
            var foundRoute = await _context.Routes.Where(route => route.Id == id).Include(r => r.Routeparts).FirstOrDefaultAsync();
            if (foundRoute == null)
            {
                return null;
            }
            return foundRoute;
        }

        public bool RouteExists(int routeId)
        {
            bool routeExists = _context.Routes.Any(e => e.Id == routeId);
            return routeExists;
        }

        public async Task<TWRoute> CreateRouteOnEditionAsync(TWRoute route)
        {
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();

            return route;
        }

        public async Task<TWRoute> UpdateRouteOnEditionAsync(TWRoute existingRoute, UpdateRouteDto updatedRoute)
        {
            if (updatedRoute.Name != null)
            {
                existingRoute.Name = updatedRoute.Name;
                _context.Routes.Update(existingRoute);
            }

            await _context.SaveChangesAsync();

            return existingRoute;
        }

        public async Task<bool> DeleteRouteByIdAsync(int routeId)
        {
            _context.Routes.Remove(await GetRouteByIdAsync(routeId));
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
