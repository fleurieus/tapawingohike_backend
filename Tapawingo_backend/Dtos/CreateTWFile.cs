namespace Tapawingo_backend.Dtos
{
    public class CreateTWFile
    {
        public string FileName { get; set; }
        public string Category { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
