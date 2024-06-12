using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Interface
{
    public interface IOrganisationsRepository
    {
        Organisation CreateOrganisation(CreateOrganisationDto model);
        Task<Organisation> GetOrganisationById(int id);
        List<Organisation> GetAllOrganisations();
        bool OrganisationExists(int id);
        Task<Organisation> UpdateOrganisation(int id, UpdateOrganisationDto newOrganisation);
    }
}
