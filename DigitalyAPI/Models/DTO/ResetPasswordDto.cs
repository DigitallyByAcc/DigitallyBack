using System.ComponentModel.DataAnnotations;

namespace DigitalyAPI.Models.DTO
{
    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 8, ErrorMessage = " New  Password  must be at least {2} , and maximum {15} characters   ")]

        public string NewPassword { get; set; }
    }
}
