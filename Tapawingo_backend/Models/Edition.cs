namespace Tapawingo_backend.Models
{
    public class Edition
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<TWRoute> Routes { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}
