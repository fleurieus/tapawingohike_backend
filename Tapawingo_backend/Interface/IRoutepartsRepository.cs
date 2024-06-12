using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IRoutepartsRepository
    {
        Task<Routepart> CreateRoutePart(Routepart newRoutepart);
    }
}
