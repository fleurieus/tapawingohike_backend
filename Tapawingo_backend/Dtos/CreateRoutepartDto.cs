using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Dtos
{
    public class CreateRoutepartDto
    {
        public required string Name { get; set; }
        public required string RouteType { get; set; }
        public bool RoutepartZoom { get; set; }
        public bool RoutepartFullscreen { get; set; }
        public bool Final {  get; set; }
        [FromForm(Name = "destinations")]
        public string? DestinationsJson { get; set; }
        public List<CreateDestinationDto>? Destinations
        {
            get => string.IsNullOrEmpty(DestinationsJson) ? null : JsonConvert.DeserializeObject<List<CreateDestinationDto>>(DestinationsJson);
        }
        public ICollection<IFormFile>? Files { get; set; }
    }
}
