using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface ILocationlogsRepository
    {
        Task<Locationlog> CreateLocationlogOnTeamAsync(Locationlog locationlog);
    }
}
