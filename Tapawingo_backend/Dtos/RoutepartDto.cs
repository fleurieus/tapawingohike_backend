using System.Text.Json.Serialization;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Dtos
{
    public class RoutepartDto
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public string Name { get; set; }
        public string RouteType { get; set; }
        public bool RoutepartZoom { get; set; }
        public bool RoutepartFullscreen { get; set; }
        public int Order { get; set; }
        public bool Final { get; set; }
        public ICollection<DestinationDto>? Destinations { get; set; }
        public ICollection<TWFileDto>? Files { get; set; }
    }
}
