using System.ComponentModel.DataAnnotations;

namespace DigitalyAPI.Models.DTO
{
    public class ConfirmEmailDto
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string Email { get; set; }

    }
}
