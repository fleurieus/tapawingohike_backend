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
        public RouteDto GetRouteById(int id)
        {
            var foundRoute = _context.Routes.FirstOrDefault(route => route.Id == id);
            if (foundRoute == null)
            {
                return null;
            }
            return new RouteDto
            {
                Id = foundRoute.Id,
                Name = foundRoute.Name,
                EditionId = foundRoute.EditionId
            };
        }

        public RouteDto CreateRoute(CreateRouteDto createRouteDto)
        {
            var newRoute = new TWRoute()
            {
                Name = createRouteDto.Name,
                EditionId = createRouteDto.EditionId
            };
            _context.Routes.Add(newRoute);
            _context.SaveChanges();
            return new RouteDto()
            {
                Id = newRoute.Id,
                Name = newRoute.Name,
                EditionId = newRoute.EditionId
            };
        }
    }
}
