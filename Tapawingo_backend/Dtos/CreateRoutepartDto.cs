using Tapawingo_backend.Models;

namespace Tapawingo_backend.Dtos
{
    public class CreateRoutepartDto
    {
        public required string Name { get; set; }
        public required string RouteType { get; set; }
        public bool RoutepartZoom { get; set; }
        public bool RoutepartFullscreen { get; set; }
        public int Order { get; set; }
        public bool Final {  get; set; }
        public ICollection<DestinationDto>? Destinations { get; set; }
        public ICollection<TWFileDto>? Files { get; set; }

    }
}
