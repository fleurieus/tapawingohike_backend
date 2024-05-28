using System.Text.Json.Serialization;

namespace Tapawingo_backend.Models
{
    public class TeamRoutepart
    {
        [JsonIgnore]
        public int TeamId { get; set; }
        [JsonIgnore]
        public int RoutepartId { get; set; }
        [JsonIgnore]
        public Team Team { get; set; }
        public Routepart Routepart { get; set; }
        public bool IsFinished { get; set; }
        public DateTime CompletedTime { get; set; }
    }
}
