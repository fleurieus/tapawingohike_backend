using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;

namespace Tapawingo_backend.Services
{
    public class OrganisationsService
    {
        private readonly IOrganisationsRepository _organisationsRepository;
        private readonly IMapper _mapper;

        public OrganisationsService(IOrganisationsRepository organisationsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _organisationsRepository = organisationsRepository;
        }

        public async Task<List<OrganisationDto>> GetOrganisations()
        {
            return _mapper.Map<List<OrganisationDto>>(await _organisationsRepository.GetAllOrganisations());
        }

        public async Task<OrganisationDto> CreateOrganisation(CreateOrganisationDto organisationName)
        {
            return _mapper.Map<OrganisationDto>(await _organisationsRepository.CreateOrganisation(organisationName));
        }

        public async Task<OrganisationDto> GetOrganisationById(int id)
        {
            return _mapper.Map<OrganisationDto>(await _organisationsRepository.GetOrganisationById(id));
        }

        public async Task<OrganisationDto> UpdateOrganisation(int id, UpdateOrganisationDto newOrganisation)
        {
            if(await _organisationsRepository.GetOrganisationById(id) == null)
            {
                return null;
            }
            return _mapper.Map<OrganisationDto>(await _organisationsRepository.UpdateOrganisationAsync(id, newOrganisation));
        }

        public async Task<IActionResult> DeleteOrganisationAsync(int id)
        {
            var targetOrganisation = await _organisationsRepository.GetOrganisationById(id);
            if (targetOrganisation == null)
                return new NotFoundObjectResult(new { message = "Organisation not found" });
            var deleteOperationSuccesfull = await _organisationsRepository.DeleteOrganisationAsync(id);
            return deleteOperationSuccesfull ?
                new NoContentResult() :
                new BadRequestObjectResult(new { message = "This request could not be handled" });
        }
    }
}
