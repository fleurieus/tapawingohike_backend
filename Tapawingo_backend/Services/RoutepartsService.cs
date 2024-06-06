using AutoMapper;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Services
{
    public class RoutepartsService
    {
        private readonly IMapper _mapper;
        IRoutepartsRepository _routepartsRepository;
        IRoutesRepository _routesRepository;

        public RoutepartsService(IMapper mapper, IRoutepartsRepository routepartsRepository, IRoutesRepository routesRepository)
        {
            _mapper = mapper;
            _routepartsRepository = routepartsRepository;
            _routesRepository = routesRepository;
        }

        public Routepart CreateRoutepart(CreateRoutepartDto routepart, int routeId) 
        {
            if (!_routesRepository.RouteExists(routeId))
            {
                throw new InvalidOperationException("Route does not exist");
            }

            if (string.IsNullOrEmpty(routepart.Name))
            {
                throw new ArgumentException("Route name is required");
            }

            if (string.IsNullOrEmpty(routepart.RouteType))
            {
                throw new ArgumentException("Routetype is required");
            }

            var routeEntity = _mapper.Map<Routepart>(routepart);
            routeEntity.RouteId = routeId;
            return _routepartsRepository.CreateRoutePart(routeEntity);
        }
    }
}
