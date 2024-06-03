using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IRoutesRepository
    {
        TWRoute GetRouteById(int id);
    }
}
