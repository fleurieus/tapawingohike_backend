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
            CreateMap<Event, EventDto>();
            CreateMap<Organisation, OrganisationDto>();
            CreateMap<Route, RouteDto>();
        }
    }
}
