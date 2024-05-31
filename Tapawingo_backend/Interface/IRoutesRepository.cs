using Tapawingo_backend.Dtos;

namespace Tapawingo_backend.Interface
{
    public interface IRoutesRepository
    {
        RouteDto GetRouteById(int id);
    }
}
