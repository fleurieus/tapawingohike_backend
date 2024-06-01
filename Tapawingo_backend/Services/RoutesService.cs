using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;

namespace Tapawingo_backend.Services
{
    public class RoutesService
    {
        private readonly IRoutesRepository _routesRepository;

        public RoutesService(IRoutesRepository routesRepository) {
            _routesRepository = routesRepository;
        }

        public RouteDto GetRoutesById(int id)
        {
            return _routesRepository.GetRouteById(id);
        }
        
        public RouteDto CreateRoute(CreateRouteDto createRouteDto)
        {
            return _routesRepository.CreateRoute(createRouteDto);
        }
    }
}
