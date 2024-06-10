namespace Tapawingo_backend.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int OrganisationId { get; set; }
        public Organisation Organisation { get; set; }
        public string Name { get; set; }
        public ICollection<UserEvent> Users { get; set; }
        public ICollection<Edition> Editions { get; set; }
    }
}
