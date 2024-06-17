using System.ComponentModel.DataAnnotations;

namespace Tapawingo_backend.Dtos
{
    public class CreateUserOnEventDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
