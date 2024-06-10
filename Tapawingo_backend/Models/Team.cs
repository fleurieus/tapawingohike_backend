namespace Tapawingo_backend.Models
{
    public class Team
    {
        public int Id { get; set; }
        public int EditionId { get; set; }
        public Edition Edition { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public bool Online { get; set; }
        public ICollection<Locationlog> Locationlogs { get; set; }
        public ICollection<TeamRoutepart> TeamRouteparts { get; set; }
    }
}
