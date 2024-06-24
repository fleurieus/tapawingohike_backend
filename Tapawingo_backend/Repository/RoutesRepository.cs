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

        public async Task<bool> RouteExists(int routeId)
        {
            bool routeExists = await _context.Routes.AnyAsync(e => e.Id == routeId);
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

        public async Task<bool> GetActiveStatus(int editionId, int routeId)
        {
            var targetRoute = await _context.Routes.FirstOrDefaultAsync(r => r.EditionId == editionId && r.Id == routeId);
            if (targetRoute == null) return false;
            return targetRoute.Active;
        }

        public async Task<bool> DeactivateRoute(int editionId, int routeId)
        {
            //find target route
            var targetRoute = await _context.Routes.FirstOrDefaultAsync(r => r.EditionId == editionId && r.Id == routeId);

            //if null, can't update and send message
            if (targetRoute == null) return false;

            //set target route to true
            targetRoute.Active = false;

            //update the route
            _context.Routes.Update(targetRoute);
            await _context.SaveChangesAsync();

            //send true
            return true;
        }

        public async Task<bool> SetActiveRoute(int editionId, int routeId)
        {
            //find target route
            var targetRoute = await _context.Routes.FirstOrDefaultAsync(r => r.EditionId == editionId && r.Id == routeId);

            //if null, can't update and send message
            if (targetRoute == null) return false;

            //if not null set all other routes for this edition to isActive = false
            var allRoutesOfEdition = await _context.Routes.Where(r => r.EditionId == editionId).ToListAsync();
            foreach (var route in allRoutesOfEdition)
            {
                route.Active = false;
                _context.Routes.Update(route);
            }
            await _context.SaveChangesAsync();

            //set target route to true
            targetRoute.Active = true;

            //update the route
            _context.Routes.Update(targetRoute);
            await _context.SaveChangesAsync();

            //send true
            return true;
        }
    }
}
