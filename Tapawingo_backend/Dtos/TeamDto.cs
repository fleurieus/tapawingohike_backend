using System.ComponentModel.DataAnnotations;
using Tapawingo_backend.Models;

namespace Tapawingo_backend.Dtos
{
    public class TeamDto
    {
        public int Id { get; set; }
        public int EditionId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ContactName { get; set; }
        [EmailAddress]
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public bool Online { get; set; }
        public ICollection<TeamRoutepartDto>? TeamRouteparts { get; set; }
    }
}
