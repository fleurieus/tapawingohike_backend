using AutoMapper;
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

        public async Task<TeamDto> CreateTeamOnEdition(int editionId, CreateTeamDto model)
        {
            if (!_editionsRepository.EditionExists(editionId))
                throw new BadHttpRequestException("Edition not found");

            // TODO check if team already exists with that name

            try
            {
                return _mapper.Map<TeamDto>(await _teamRepository.CreateTeamOnEdition(editionId, model));
            }
            catch (Exception e)
            {
                throw new BadHttpRequestException(e.Message);
            }
        }
    }
}
