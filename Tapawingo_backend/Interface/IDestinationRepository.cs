using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IDestinationRepository
    {
        Task<Destination> CreateDestination(Destination destination);
    }
}
