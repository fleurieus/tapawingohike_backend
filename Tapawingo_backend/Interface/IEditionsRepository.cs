using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IEditionsRepository
    {
        Edition GetEditionById(int eventId, int editionId);
        List<Edition> GetAllEditions(int editionId);
        Edition CreateEdition(Edition newEdition);
        bool EditionExists(int editionId);
        Task<Edition> UpdateEditionAsync(Edition existingEdition, UpdateEditionDto updateEditionDto);
        Task<bool> DeleteEditionAsync(int eventId, int editionId);
    }
}
