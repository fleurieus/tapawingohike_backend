using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IRoutesRepository
    {
        TWRoute GetRouteById(int id);
        TWRoute CreateRoute(CreateRouteDto createRouteDto);
    }
}
