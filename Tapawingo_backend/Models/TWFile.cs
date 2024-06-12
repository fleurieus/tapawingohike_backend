namespace Tapawingo_backend.Models
{
    public class TWFile
    {
        public int Id { get; set; }
        public int RoutepartId { get; set; }
        public Routepart Routepart { get; set; }
        public string File { get; set; }
        public string Category { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
