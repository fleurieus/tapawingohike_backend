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

        public async Task<RoutepartDto> CreateRoutepartAsync(CreateRoutepartDto createRoutepart, int routeId) 
        {
            TWRoute route = _routesRepository.GetRouteById(routeId);
            
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
    }
}
