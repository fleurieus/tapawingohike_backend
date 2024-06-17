using Tapawingo_backend.Models;

namespace Tapawingo_backend.Dtos
{
    public class UpdateEditionDto
    {
        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
