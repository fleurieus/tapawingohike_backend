using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;

namespace Tapawingo_backend.Services
{
    public class OrganisationsService
    {
        private readonly IOrganisationsRepository _organisationsRepository;

        public OrganisationsService(IOrganisationsRepository organisationsRepository)
        {
            _organisationsRepository = organisationsRepository;
        }

        public OrganisationDto CreateOrganisation(CreateOrganisationDto organisationName)
        {
            return _organisationsRepository.CreateOrganisation(organisationName);
        }
    }
}
