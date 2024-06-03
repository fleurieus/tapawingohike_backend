namespace Tapawingo_backend.Models
{
    public class TWRoute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Routepart> Routeparts { get; set; }
        public int Edition_id { get; set; }
        public Edition Edition { get; set; }
    }
}
