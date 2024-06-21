using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        IDestinationRepository _destinationRepository;
        IFileRepository _fileRepository;

        public RoutepartsService(IMapper mapper, IRoutepartsRepository routepartsRepository, IRoutesRepository routesRepository, IDestinationRepository destinationRepository, IFileRepository fileRepository)
        {
            _mapper = mapper;
            _routepartsRepository = routepartsRepository;
            _routesRepository = routesRepository;
            _destinationRepository = destinationRepository;
            _fileRepository = fileRepository;
        }

        public async Task<List<RoutepartDto>> GetRoutepartsAsync(int route_id)
        {
            if (!await _routesRepository.RouteExists(route_id)) return null;
            return _mapper.Map<List<RoutepartDto>>(await _routepartsRepository.GetRoutepartsAsync(route_id));
        }

        public async Task<RoutepartDto> GetRoutepartOnRouteAsync(int routeId, int routepartId)
        {
            if (!await _routesRepository.RouteExists(routeId))
                throw new BadHttpRequestException("Route not found");

            if (!await _routepartsRepository.RoutepartExists(routepartId))
                throw new BadHttpRequestException("Routepart not found");

            Routepart routepart = await _routepartsRepository.GetRoutepartOnRouteAsync(routeId, routepartId);
            if (routepart == null)
                throw new BadHttpRequestException("Routepart does not exist on route");
            else
                return _mapper.Map<RoutepartDto>(await _routepartsRepository.GetRoutepartOnRouteAsync(routeId, routepartId));
        }

        public async Task<RoutepartDto> CreateRoutepartAsync(CreateRoutepartDto createRoutepart, int routeId) 
        {
            TWRoute route = await _routesRepository.GetRouteByIdAsync(routeId);
            
            if (route == null)
                return null;

            var newRoutepart = new Routepart()
            {
                RouteId = routeId,
                Name = createRoutepart.Name,
                RouteType = createRoutepart.RouteType,
                RoutepartZoom = createRoutepart.RoutepartZoom,
                RoutepartFullscreen = createRoutepart.RoutepartFullscreen,
                Order = route.Routeparts.Count + 1,
                Final = createRoutepart.Final,
            };

            var createdRoutepart = await _routepartsRepository.CreateRoutePartAsync(newRoutepart);

            // Add destinations if provided
            if (createRoutepart.Destinations != null && createRoutepart.Destinations.Any())
            {
                foreach (var destination in createRoutepart.Destinations)
                {
                    var newDestination = new Destination()
                    {
                        RoutepartId = createdRoutepart.Id,
                        Name = destination.Name,
                        Latitude = destination.Latitude,
                        Longitude = destination.Longitude,
                        Radius = destination.Radius,
                        DestinationType = destination.DestinationType,
                        ConfirmByUser = destination.ConfirmByUser,
                        HideForUser = destination.HideForUser
                    };

                    await _destinationRepository.CreateDestinationAsync(newDestination);
                }
            }

            // Add files if provided
            if (createRoutepart.Files != null && createRoutepart.Files.Any())
            {
                foreach (var file in createRoutepart.Files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);

                        var newFile = new TWFile
                        {
                            RoutepartId = createdRoutepart.Id,
                            File = file.FileName,
                            ContentType = file.ContentType, // This is also the category of the file
                            Data = memoryStream.ToArray(),
                            UploadDate = DateTime.UtcNow
                        };

                        await _fileRepository.SaveFileAsync(newFile);
                    }
                }
            }

            return _mapper.Map<RoutepartDto>(createdRoutepart);
        }

        public async Task<RoutepartDto> UpdateRoutepartOnRouteAsync(int routeId, int routepartId, UpdateRoutepartDto updateRoutepartDto)
        {
            if (!_routesRepository.RouteExists(routeId))
                throw new BadHttpRequestException("Route not found");

            if (!_routepartsRepository.RoutepartExists(routepartId))
                throw new BadHttpRequestException("Routepart not found");

            Routepart routepart = await _routepartsRepository.GetRoutepartOnRouteAsync(routeId, routepartId);
            if (routepart == null)
                throw new BadHttpRequestException("Routepart does not exist on route");
            else
                return _mapper.Map<RoutepartDto>(await _routepartsRepository.UpdateRoutepartOnRouteAsync(routepart, updateRoutepartDto));
        }

        public async Task<IActionResult> DeleteRoutepartOnRouteAsync(int routeId, int routepartId)
        {
            if (!await _routesRepository.RouteExists(routeId))
                return new NotFoundObjectResult(new
                {
                    message = "Route not found"
                });

            if (!await _routepartsRepository.RoutepartExists(routepartId))
                return new NotFoundObjectResult(new
                {
                    message = "Routepart not found"
                });

            Routepart routepart = await _routepartsRepository.GetRoutepartOnRouteAsync(routeId, routepartId);
            if (routepart == null)
                return new NotFoundObjectResult(new
                {
                    message = "Routepart does not exist on route"
                });

            bool routepartDeleted = await _routepartsRepository.DeleteRoutepartOnRouteAsync(routeId, routepart);
            return routepartDeleted ? new NoContentResult() : new BadRequestObjectResult(new
            {
                message = "Routepart could not be deleted on organisation"
            });
        }
    }
}
