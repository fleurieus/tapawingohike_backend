using Tapawingo_backend.Models;

namespace Tapawingo_backend.Dtos
{
    public class LocationlogDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
