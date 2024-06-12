using System.ComponentModel.DataAnnotations;

namespace Tapawingo_backend.Dtos
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required bool IsManager { get; set; }
    }
}
