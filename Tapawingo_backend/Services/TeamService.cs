using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Services
{
    public class TeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IEditionsRepository _editionsRepository;
        private readonly IMapper _mapper;

        public TeamService(ITeamRepository teamRepository, IEditionsRepository editionsRepository, IMapper mapper)
        {
            _teamRepository = teamRepository;
            _editionsRepository = editionsRepository;
            _mapper = mapper;
        }

        public List<TeamDto> GetTeamsOnEdition(int editionId)
        {
            if (!_editionsRepository.EditionExists(editionId))
                throw new BadHttpRequestException("Edition not found");

            return _mapper.Map<List<TeamDto>>(_teamRepository.GetTeamsOnEdition(editionId));
        }

        public async Task<TeamDto> GetTeamOnEditionAsync(int editionId, int teamId)
        {
            if (!_editionsRepository.EditionExists(editionId))
                throw new BadHttpRequestException("Edition not found");

            if (!_teamRepository.TeamExists(teamId))
                throw new BadHttpRequestException("Team not found");

            return _mapper.Map<TeamDto>(await _teamRepository.GetTeamOnEditionAsync(editionId, teamId));
        }

        public async Task<TeamDto> CreateTeamOnEditionAsync(int editionId, CreateTeamDto model)
        {
            if (!_editionsRepository.EditionExists(editionId))
                throw new BadHttpRequestException("Edition not found");

            // TODO check if team already exists with that name

            try
            {
                return _mapper.Map<TeamDto>(await _teamRepository.CreateTeamOnEditionAsync(editionId, model));
            }
            catch (Exception e)
            {
                throw new BadHttpRequestException(e.Message);
            }
        }
    }
}
