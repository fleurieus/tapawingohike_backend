using System.Text.Json.Serialization;

namespace Tapawingo_backend.Models
{
    public class UserEvent
    {
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonIgnore]
        public int EventId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public Event Event { get; set; }
    }
}
