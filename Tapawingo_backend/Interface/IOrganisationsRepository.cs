using Tapawingo_backend.Dtos;

namespace Tapawingo_backend.Interface
{
    public interface IOrganisationsRepository
    {
        OrganisationDto CreateOrganisation(CreateOrganisationDto createOrganisationDto);
    }
}
