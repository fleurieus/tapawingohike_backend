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

        public TWRoute GetRouteById(int id)
        {
            var foundRoute = _context.Routes.FirstOrDefault(route => route.Id == id);
            if (foundRoute == null)
            {
                return null;
            }
            return foundRoute;
        }

        public TWRoute CreateRoute(CreateRouteDto createRouteDto)
        {
            var newRoute = new TWRoute()
            {
                Name = createRouteDto.Name,
                EditionId = createRouteDto.EditionId
            };
            _context.Routes.Add(newRoute);
            _context.SaveChanges();
            return newRoute;
        }
    }
}
