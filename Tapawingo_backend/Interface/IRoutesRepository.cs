using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IRoutesRepository
    {
        Task<List<TWRoute>> GetRoutesOnEditionAsync(int editionId);
        Task<TWRoute> GetRouteByIdAsync(int id);
        Task<bool> RouteExists(int routeId);
        Task<TWRoute> CreateRouteOnEditionAsync(TWRoute route);
        Task<TWRoute> UpdateRouteOnEditionAsync(TWRoute existingRoute, UpdateRouteDto updatedRoute);
        Task<bool> DeleteRouteByIdAsync(int routeId);
        Task<bool> SetActiveRoute(int editionId, int routeId);
        Task<bool> GetActiveStatus(int editionId, int routeId);
        Task<bool> DeactivateRoute(int editionId, int routeId);
    }
}
