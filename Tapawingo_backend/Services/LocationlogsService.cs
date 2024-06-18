using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;

namespace Tapawingo_backend.Services
{
    public class LocationlogsService
    {
        private readonly ILocationlogsRepository _locationlogsRepository;
        private readonly ITeamRepository _teamsRepository;
        private readonly IMapper _mapper;

        public LocationlogsService(ILocationlogsRepository locationlogsRepository, ITeamRepository teamsRepository ,IMapper mapper)
        {
            _locationlogsRepository = locationlogsRepository;
            _teamsRepository = teamsRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetLocationlogsOnTeamAsync(int teamId)
        {
            if (!_teamsRepository.TeamExists(teamId))
                return new NotFoundObjectResult(new
                {
                    message = "Team not found"
                });

            var locationlogs = _mapper.Map<List<LocationlogDto>>(await _locationlogsRepository.GetLocationlogsOnTeamAsync(teamId));
            return new OkObjectResult(locationlogs);
        }

        public async Task<LocationlogDto> CreateLocationlogOnTeamAsync(int teamId, CreateLocationlogDto createLocationlogDto)
        {
            if (!_teamsRepository.TeamExists(teamId))
                throw new BadHttpRequestException("Team not found");

            Locationlog locationlog = new Locationlog
            {
                TeamId = teamId,
                Latitude = createLocationlogDto.Latitude,
                Longitude = createLocationlogDto.Longitude,
                Timestamp = DateTime.Now
            };

            return _mapper.Map<LocationlogDto>(await _locationlogsRepository.CreateLocationlogOnTeamAsync(locationlog));
        }   
    }
}
