﻿using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects.IdentityDtos
{
    public class RegisterDto
    {
        [EmailAddress]
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? UserName { get; set; } = "ayawaheed";
        public string DisplayName { get; set; } = default!;
        [Phone]
        public string? PhoneNumber { get; set; } 

    }
}
