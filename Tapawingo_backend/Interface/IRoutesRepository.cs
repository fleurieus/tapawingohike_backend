using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IRoutesRepository
    {
        Task<List<TWRoute>> GetRoutesAsync();
        Task<TWRoute> GetRouteByIdAsync(int id);
        bool RouteExists(int routeId);
        Task<bool> DeleteRouteByIdAsync(int routeId);
    }
}
