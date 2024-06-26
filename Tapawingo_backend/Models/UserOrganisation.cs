using System.Text.Json.Serialization;

namespace Tapawingo_backend.Models
{
    public class UserOrganisation
    {
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonIgnore]
        public int OrganisationId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public Organisation Organisation { get; set; }
        public bool IsManager { get; set; }
    }
}
