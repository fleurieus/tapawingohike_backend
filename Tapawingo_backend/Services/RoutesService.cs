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
        private readonly IEditionsRepository _editionsRepository;
        private readonly IMapper _mapper;

        public RoutesService(IRoutesRepository routesRepository, IEditionsRepository editionsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _routesRepository = routesRepository;
            _editionsRepository = editionsRepository;
        }

        public async Task<List<RouteDto>> GetRoutes(int editionId)
        {
            if (!_editionsRepository.EditionExists(editionId)) throw new ArgumentException("Edition not found");
            return _mapper.Map<List<RouteDto>>(await _routesRepository.GetRoutesAsync());
        }

        public async Task<RouteDto> GetRoutesById(int editionId, int routeId)
        {
            if (!_editionsRepository.EditionExists(editionId)) throw new ArgumentException("Edition not found");
            var foundRoute = await _routesRepository.GetRouteByIdAsync(routeId);
            if (foundRoute == null) return null;
            if (foundRoute.EditionId != editionId) throw new ArgumentException("Route does not exist on Edition");

            return _mapper.Map<RouteDto>(foundRoute);
        }

        public async Task<bool> DeleteRouteById(int editionId, int routeId)
        {
            if (!_editionsRepository.EditionExists(editionId)) throw new ArgumentException("Edition not found");
            var foundRoute = await _routesRepository.GetRouteByIdAsync(routeId);
            if (foundRoute == null) throw new ArgumentException("Route not found"); ;
            if (foundRoute.EditionId != editionId) throw new ArgumentException("Route does not exist on Edition");

            return await _routesRepository.DeleteRouteByIdAsync(routeId);
        }
    }
}
