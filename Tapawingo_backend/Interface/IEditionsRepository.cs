using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IEditionsRepository
    {
        Edition CreateEdition(Edition newEdition);
    }
}
