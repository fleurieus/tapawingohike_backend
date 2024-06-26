using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;

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

        public async Task<List<RouteDto>> GetRoutesOnEditionAsync(int editionId)
        {
            if (!await _editionsRepository.EditionExists(editionId)) throw new ArgumentException("Edition not found");
            return _mapper.Map<List<RouteDto>>(await _routesRepository.GetRoutesOnEditionAsync(editionId));
        }

        public async Task<RouteDto> GetRouteOnEditionAsync(int editionId, int routeId)
        {
            if (!await _editionsRepository.EditionExists(editionId)) throw new ArgumentException("Edition not found");
            var foundRoute = await _routesRepository.GetRouteByIdAsync(routeId);
            if (foundRoute == null) return null;
            if (foundRoute.EditionId != editionId) throw new ArgumentException("Route does not exist on Edition");

            return _mapper.Map<RouteDto>(foundRoute);
        }

        public async Task<RouteDto> CreateRouteOnEditionAsync(int editionId, CreateRouteDto createRouteDto)
        {
            if (!await _editionsRepository.EditionExists(editionId))
                throw new BadHttpRequestException("Edition not found");

            TWRoute route = new TWRoute
            {
                EditionId = editionId,
                Name = createRouteDto.Name,
            };

            return _mapper.Map<RouteDto>(await _routesRepository.CreateRouteOnEditionAsync(route));
        }

        public async Task<RouteDto> UpdateRouteOnEditionAsync(int editionId, int routeId, UpdateRouteDto model)
        {
            if (!await _editionsRepository.EditionExists(editionId))
                throw new BadHttpRequestException("Edition not found");

            if (!await _routesRepository.RouteExists(routeId))
                throw new BadHttpRequestException("Route not found");

            var route = await _routesRepository.GetRouteByIdAsync(routeId);
            if (route.EditionId != editionId)
                throw new BadHttpRequestException("Route does not exist on Edition");

            return _mapper.Map<RouteDto>(await _routesRepository.UpdateRouteOnEditionAsync(route, model));
        }

        public async Task<bool> DeleteRouteById(int editionId, int routeId)
        {
            if (!await _editionsRepository.EditionExists(editionId)) throw new ArgumentException("Edition not found");
            var foundRoute = await _routesRepository.GetRouteByIdAsync(routeId);
            if (foundRoute == null) throw new ArgumentException("Route not found"); ;
            if (foundRoute.EditionId != editionId) throw new ArgumentException("Route does not exist on Edition");

            return await _routesRepository.DeleteRouteByIdAsync(routeId);
        }

        public async Task<IActionResult> SetActiveRoute(int editionId, int routeId)
        {
            if (!await _editionsRepository.EditionExists(editionId)) return new BadRequestObjectResult(new
            {
                message = "Edition not found"
            });
            if (!await _routesRepository.RouteExists(routeId)) return new BadRequestObjectResult(new
            {
                message = $"Route does not exist"
            });
            if ((await _routesRepository.GetRouteByIdAsync(routeId)).EditionId != editionId) return new BadRequestObjectResult(new
            {
                message = "Route does not exist on Edition"
            });

            var targetRotue = await _routesRepository.GetRouteByIdAsync(routeId);

            //check if the route is active
            if(await _routesRepository.GetActiveStatus(editionId, routeId))
            {
                //route is already active, so deactive it
                var deactivated = await _routesRepository.DeactivateRoute(editionId, routeId);
                return deactivated ? new OkObjectResult(new
                {
                    message = $"Route \'{targetRotue.Name}\' is now deactivated"
                }) : new BadRequestObjectResult(new
                {
                    message = $"Route '{targetRotue.Name}\' could not been deactivated"
                });
            }else
            {
                var activated = await _routesRepository.SetActiveRoute(editionId, routeId);
                return activated ? new OkObjectResult(new
                {
                    message = $"Route '{targetRotue.Name}\' is now active."
                }) : new BadRequestObjectResult(new
                {
                    message = $"Route '{targetRotue.Name}\' could not be activated."
                });
            }
            
        }
    }
}
