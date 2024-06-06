using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IRoutepartsRepository
    {
        Routepart CreateRoutePart(Routepart newRoutepart);
    }
}
