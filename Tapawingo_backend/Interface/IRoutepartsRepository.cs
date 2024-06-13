using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IRoutepartsRepository
    {
        Task<Routepart> CreateRoutePartAsync(Routepart newRoutepart);
        Task<List<Routepart>> GetRoutepartsAsync(int route_id);
    }
}
