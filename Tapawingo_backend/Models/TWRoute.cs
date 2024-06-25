namespace Tapawingo_backend.Models
{
    public class TWRoute
    {
        public int Id { get; set; }
        public int EditionId { get; set; }
        public Edition Edition { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public ICollection<Routepart> Routeparts { get; set; }
    }
}
