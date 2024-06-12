using Tapawingo_backend.Models;

namespace Tapawingo_backend.Dtos
{
    public class TWFileDto
    {
        public int Id { get; set; }
        public int RoutepartId { get; set; }
        public string File { get; set; }
        public string Category { get; set; }
    }
}
