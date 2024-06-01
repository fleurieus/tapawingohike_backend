using System.ComponentModel.DataAnnotations;

namespace Tapawingo_backend.Dtos
{
    public class RouteDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int EditionId { get; set; }
    }
}
