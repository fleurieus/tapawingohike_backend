using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IOrganisationsRepository
    {
        Task<Organisation> CreateOrganisation(CreateOrganisationDto model);
        Task<Organisation> GetOrganisationById(int id);
        Task<List<Organisation>> GetAllOrganisations(int? organisationId);
        Task<bool> OrganisationExists(int id);
        Task<Organisation> UpdateOrganisationAsync(int id, UpdateOrganisationDto newOrganisation);
        Task<bool> DeleteOrganisationAsync(int id);
    }
}
