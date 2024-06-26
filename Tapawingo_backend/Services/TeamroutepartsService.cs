using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Services
{
    public class TeamroutepartsService
    {
        private readonly ITeamroutepartsRepository _teamroutepartsRepository;
        private readonly IMapper _mapper;

        public TeamroutepartsService(ITeamroutepartsRepository teamroutepartsRepository, IMapper mapper)
        {
            _teamroutepartsRepository = teamroutepartsRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> UpdateTeamRoutepart(int teamId, int routepartId, bool finished)
        {
            if (teamId == 0 || routepartId == 0) return new BadRequestObjectResult(new
            {
                message = "TeamId and RoutepartId should not be null/0"
            });

            var updatedTeamRoutePart = await _teamroutepartsRepository.UpdateTeamRoutePart(teamId, routepartId, finished);

            return updatedTeamRoutePart == null ?
                new NotFoundObjectResult(new
                {
                    message = "The teamroutepart has not been found"
                }) :
                new OkObjectResult(_mapper.Map<TeamRoutepartDto>(updatedTeamRoutePart));
        }
    }
}
