using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IEditionsRepository
    {
        Task<Edition> GetEditionById(int editionId);
        Task<List<Edition>> GetAllEditions(int editionId);
        Task<Edition> CreateEditionOnEventAsync(Edition edition);
        Task<bool> EditionExists(int editionId);
        Task<Edition> UpdateEditionAsync(Edition existingEdition, UpdateEditionDto updateEditionDto);
        Task<bool> DeleteEditionAsync(int editionId);
        Task<bool> EventExistsOnEdition(int eventId, int editionId);
    }
}
