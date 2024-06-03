namespace Tapawingo_backend.Models
{
    // Model class representing a team
    public class Team
    {
        // Unique identifier for the team
        public int Id { get; set; }

        // Name of the team
        public string Name { get; set; }

        // Code associated with the team
        public string Code { get; set; }

        // Name of the contact person for the team
        public string ContactName { get; set; }

        // Email address of the contact person for the team
        public string ContactEmail { get; set; }

        // Phone number of the contact person for the team
        public string ContactPhone { get; set; }

        // Indicates whether the team operates online
        public bool Online { get; set; }

        // Collection of location logs associated with the team
        public ICollection<Locationlog> Locationlogs { get; set; }

        // Collection of route parts associated with the team
        public ICollection<TeamRoutepart> TeamRouteparts { get; set; }

        // The Id of the edition to which the team is associated to
        public int EditionId { get; set; }

        // The referential object of the Edition
        public Edition Edition { get; set; }
    }
}
