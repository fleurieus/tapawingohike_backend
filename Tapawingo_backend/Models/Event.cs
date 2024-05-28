namespace Tapawingo_backend.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserEvent> Users { get; set; }
        public ICollection<Edition> Editions { get; set; }
    }
}
