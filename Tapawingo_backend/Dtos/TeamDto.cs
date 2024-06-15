using System.ComponentModel.DataAnnotations;

namespace Tapawingo_backend.Dtos
{
    public class TeamDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int EditionId { get; set; }
        public string Code { get; set; }
        public string ContactName { get; set; }
        [EmailAddress]
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public bool Online { get; set; }
    }
}
