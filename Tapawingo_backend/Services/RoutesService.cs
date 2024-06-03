using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;

namespace Tapawingo_backend.Services
{
    public class RoutesService
    {
        private readonly IRoutesRepository _routesRepository;
        private readonly IMapper _mapper;

        public RoutesService(IRoutesRepository routesRepository, IMapper mapper)
        {
            _mapper = mapper;
            _routesRepository = routesRepository;
        }

        public RouteDto GetRoutesById(int id)
        {
            return _mapper.Map<RouteDto>(_routesRepository.GetRouteById(id));
        }
    }
}
