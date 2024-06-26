using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Tapawingo_backend.Dtos
{
    public class UpdateRoutepartDto
    {
        public string Name { get; set; }
        public string RouteType { get; set; }
        public bool RoutepartZoom { get; set; }
        public bool RoutepartFullscreen { get; set; }
        public bool Final { get; set; }
        [FromForm(Name = "destinations")]
        public string? DestinationsJson { get; set; }
        public List<UpdateDestinationDto>? Destinations
        {
            get => string.IsNullOrEmpty(DestinationsJson) ? null : JsonConvert.DeserializeObject<List<UpdateDestinationDto>>(DestinationsJson);
        }
        public ICollection<IFormFile>? Files { get; set; }
    }
}
