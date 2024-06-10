namespace Tapawingo_backend.Dtos
{
    public class CreateEditionDto
    {
        public required string Name { get; set; }
        public required DateTime StartDate {  get; set; }
        public required DateTime EndDate { get; set; }
    }
}
