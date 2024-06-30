using AutoMapper;
using Google.Protobuf.Reflection;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Services
{
    public class TeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IEditionsRepository _editionsRepository;
        private readonly IRoutepartsRepository _routepartsRepository;
        private readonly IMapper _mapper;

        public TeamService(ITeamRepository teamRepository, IEditionsRepository editionsRepository, IRoutepartsRepository routepartsRepository, IMapper mapper)
        {
            _teamRepository = teamRepository;
            _editionsRepository = editionsRepository;
            _routepartsRepository = routepartsRepository;
            _mapper = mapper;
        }

        public async Task<List<TeamDto>> GetTeamsOnEdition(int editionId)
        {
            if (!await _editionsRepository.EditionExists(editionId))
                throw new BadHttpRequestException("Edition not found");

            return _mapper.Map<List<TeamDto>>(await _teamRepository.GetTeamsOnEdition(editionId));
        }

        public async Task<TeamDto> GetTeamOnEditionAsync(int editionId, int teamId)
        {
            if (!await _editionsRepository.EditionExists(editionId))
                throw new BadHttpRequestException("Edition not found");

            if (!await _teamRepository.TeamExists(teamId))
                throw new BadHttpRequestException("Team not found");
            if (!await _teamRepository.TeamExistsOnEdition(teamId, editionId))
                throw new BadHttpRequestException("Team does not exist on edition");

            return _mapper.Map<TeamDto>(await _teamRepository.GetTeamOnEditionAsync(editionId, teamId));
        }

        public async Task<IActionResult> GetTeamRouteparts(int editionId, int teamId)
        {
            if (!await _editionsRepository.EditionExists(editionId))
                return new NotFoundObjectResult(new { message = "Edition not found" });

            if (!await _teamRepository.TeamExists(teamId))
                return new NotFoundObjectResult(new { message = "Team not found" });
            if (!await _teamRepository.TeamExistsOnEdition(teamId, editionId))
                return new NotFoundObjectResult(new { message = "Team does not exist on edition" });

            var team = await _teamRepository.GetTeamOnEditionAsync(editionId, teamId);
            return new OkObjectResult(team.TeamRouteparts);
        }

        public async Task<TeamDto> CreateTeamOnEditionAsync(int editionId, CreateTeamDto model)
        {
            if (!await _editionsRepository.EditionExists(editionId))
                throw new BadHttpRequestException("Edition not found");
            if(!await _teamRepository.TeamcodeIsUnique(model.Code))
            {
                throw new ArgumentException("Team code already exists");
            }

            // TODO check if team already exists with that name

            try
            {
                var team = await _teamRepository.CreateTeamOnEditionAsync(editionId, model);
                await _routepartsRepository.SyncTeamRoutePartsBasedOnTeam(editionId, team.Id);
                return _mapper.Map<TeamDto>(team);
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<TeamDto> UpdateTeamOnEditionAsync(int editionId, int teamId, UpdateTeamDto model)
        {
            if (!await _editionsRepository.EditionExists(editionId))
                throw new BadHttpRequestException("Edition not found");

            if (!await _teamRepository.TeamExists(teamId))
                throw new BadHttpRequestException("Team not found");
            if (!await _teamRepository.TeamExistsOnEdition(teamId, editionId))
                throw new BadHttpRequestException("Team does not exist on edition");
            if(model.Code != null)
            {
                if (!await _teamRepository.TeamcodeIsUnique(model.Code))
                {
                    throw new ArgumentException("Team code already exists");
                }
            }

            await _teamRepository.UpdateTeamOnEditionAsync(await _teamRepository.GetTeamOnEditionAsync(editionId, teamId), model);

            return _mapper.Map<TeamDto>(await _teamRepository.GetTeamOnEditionAsync(editionId, teamId));
        }

        public async Task<IActionResult> DeleteTeamOnEditionAsync(int editionId, int teamId)
        {
            if (!await _editionsRepository.EditionExists(editionId))
                return new NotFoundObjectResult(new
                {
                    message = "Edition not found"
                });

            if (!await _teamRepository.TeamExists(teamId))
                return new NotFoundObjectResult(new
                {
                    message = "Team not found"
                });
            if (!await _teamRepository.TeamExistsOnEdition(teamId, editionId))
                return new ConflictObjectResult(new
                {
                    message = "Team does not exist on edition"
                });

            bool teamDeleted = await _teamRepository.DeleteTeamOnEditionAsync(editionId, teamId);
            return teamDeleted ? new NoContentResult() : new BadRequestObjectResult(new
            {
                message = "Team could not be deleted"
            });
        }

        public async Task<IActionResult> LoginWithTeamCode(string teamCode)
        {
            if(teamCode == null || teamCode.Equals(""))
            {
                return new BadRequestObjectResult(new
                {
                    message = "Teamcode cannot be empty"
                });
            }

            var team = await _teamRepository.GetTeamByTeamCode(teamCode);

            return team == null ?
                new NotFoundObjectResult(new
                {
                    message = "No team found with this team code"
                }) :
                new OkObjectResult(_mapper.Map<TeamDto>(team));
        }
    }
}
