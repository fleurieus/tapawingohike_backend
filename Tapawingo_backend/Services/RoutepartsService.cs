using AutoMapper;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;

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

        public RoutepartDto CreateRoutepart(CreateRoutepartDto createRoutepart, int routeId) 
        {
            if (!_routesRepository.RouteExists(routeId))
                return null;

            var newRoutepart = new Routepart()
            {
                RouteId = routeId,
                Name = createRoutepart.Name,
                RouteType = createRoutepart.RouteType,
                RoutepartZoom = createRoutepart.RoutepartZoom,
                RoutepartFullscreen = createRoutepart.RoutepartFullscreen,
                Order = 1,
                Final = createRoutepart.Final,
            };

            return _mapper.Map<RoutepartDto>(_routepartsRepository.CreateRoutePart(newRoutepart));
        }
    }
}
