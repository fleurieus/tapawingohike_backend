using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IEditionsRepository
    {
        List<Edition> GetAllEditions(int editionId);
        Edition CreateEdition(Edition newEdition);
    }
}
