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

        public List<OrganisationDto> GetOrganisations()
        {
            return _mapper.Map<List<OrganisationDto>>(_organisationsRepository.GetAllOrganisations());
        }

        public OrganisationDto CreateOrganisation(CreateOrganisationDto organisationName)
        {
            return _mapper.Map<OrganisationDto>(_organisationsRepository.CreateOrganisation(organisationName));
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
            return _mapper.Map<OrganisationDto>(await _organisationsRepository.UpdateOrganisation(id, newOrganisation));
        }
    }
}
