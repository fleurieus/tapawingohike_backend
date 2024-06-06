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

        public List<TWRoute> GetRoutes()
        {
            return _context.Routes.ToList();
        }

        public TWRoute GetRouteById(int id)
        {
            var foundRoute = _context.Routes.FirstOrDefault(route => route.Id == id);
            if (foundRoute == null)
            {
                return null;
            }
            return foundRoute;
        }

        public bool RouteExists(int routeId)
        {
            bool routeExists = _context.Events.Any(e => e.Id == routeId);
            return routeExists;
        }
    }
}
