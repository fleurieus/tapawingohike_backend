using System.ComponentModel.DataAnnotations; // Importing the namespace for data annotations

namespace Tapawingo_backend.Dtos
{
    // Data Transfer Object (DTO) for creating a team
    public class CreateTeamDto
    {
        // Name of the team
        public string Name { get; set; }

        // Code associated with the team
        public string Code { get; set; }

        // Name of the contact person for the team
        public string ContactName { get; set; }

        // Email address of the contact person for the team
        [EmailAddress] // Specifies that this property must be a valid email address
        public string ContactEmail { get; set; }

        // Phone number of the contact person for the team
        public string ContactPhone { get; set; }

        // Indicates whether the team operates online
        public bool Online { get; set; }

         // The Editon Id of the Edition to which the team belongs
        public int EventId { get; set; }
    }
}
