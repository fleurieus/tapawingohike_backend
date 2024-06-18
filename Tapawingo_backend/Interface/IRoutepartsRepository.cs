using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IRoutepartsRepository
    {
        Task<List<Routepart>> GetRoutepartsAsync(int route_id);
        Task<Routepart> GetRoutepartOnRouteAsync(int routeId, int routepartId);
        bool RoutepartExists(int routepartId);
        Task<Routepart> CreateRoutePartAsync(Routepart newRoutepart);
    }
}
