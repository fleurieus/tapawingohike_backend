using Tapawingo_backend.Models;

namespace Tapawingo_backend.Dtos
{
    public class EditionDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<TWRoute> Routes { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}
