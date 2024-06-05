using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IRoutesRepository
    {
        List<TWRoute> GetRoutes();
        TWRoute GetRouteById(int id);
    }
}
