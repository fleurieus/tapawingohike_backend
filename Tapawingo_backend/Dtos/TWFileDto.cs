using Tapawingo_backend.Models;

namespace Tapawingo_backend.Dtos
{
    public class TWFileDto
    {
        public int Id { get; set; }
        public int RoutepartId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
