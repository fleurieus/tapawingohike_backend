namespace Tapawingo_backend.Dtos
{
    public class UserOnOrganisationDto
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required bool IsManager { get; set; }
    }
}
