using Tapawingo_backend.Models;

namespace Tapawingo_backend.Dtos
{
    public class TeamRoutepartDto
    {
        public RoutepartDto Routepart { get; set; }
        public bool IsFinished { get; set; }
        public DateTime CompletedTime { get; set; } 
    }
}
