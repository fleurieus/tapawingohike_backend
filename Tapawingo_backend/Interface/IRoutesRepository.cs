using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IRoutesRepository
    {
        Task<List<TWRoute>> GetRoutesOnEditionAsync(int editionId);
        Task<TWRoute> GetRouteByIdAsync(int id);
        bool RouteExists(int routeId);
        Task<TWRoute> CreateRouteOnEditionAsync(TWRoute route);
        Task<bool> DeleteRouteByIdAsync(int routeId);
    }
}
