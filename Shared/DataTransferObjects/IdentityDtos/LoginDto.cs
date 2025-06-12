using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects.IdentityDtos
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
