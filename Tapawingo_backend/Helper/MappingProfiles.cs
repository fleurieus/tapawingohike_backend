using AutoMapper;
using Tapawingo_backend.Dtos;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Event, EventDto>();
            CreateMap<Organisation, OrganisationDto>();
            CreateMap<CreateEventDto, Event>();
            CreateMap<CreateEditionDto, Edition>();
            CreateMap<TWRoute, RouteDto>();
            CreateMap<Team, TeamDto>();
            CreateMap<Edition, EditionDto>();
            CreateMap<UpdateOrganisationDto, OrganisationDto>();
            CreateMap<Routepart, RoutepartDto>();
            CreateMap<Destination, DestinationDto>();
            CreateMap<TWFile, TWFileDto>();
            CreateMap<Locationlog, LocationlogDto>();
            CreateMap<TeamRoutepart, TeamRoutepartDto>();
        }
    }
}
