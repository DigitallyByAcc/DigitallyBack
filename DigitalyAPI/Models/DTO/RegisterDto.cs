using System.ComponentModel.DataAnnotations;

namespace DigitalyAPI.Models.DTO
{
    public class RegisterDto
    {
        [Required]
        [StringLength(15,MinimumLength =2,ErrorMessage =" First Name must be at least {2} , and maximum {1} characters   ")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 2, ErrorMessage = " First Name must be at least {2} , and maximum {1} characters   ")]

        public string LastName { get; set; }

        [Required]
       // [RegularExpression("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$", ErrorMessage =" Invalid email adress")]
        public string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 8, ErrorMessage = " Password  must be at least {2} , and maximum {15} characters   ")]

        public string Password { get; set; }



        public string PhoneNumber { get; set; }
        public string PhoneFix { get; set; }
        public string Fax { get; set; }
        public string Civilite { get; set; }
        public string Dateofbirth { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Grade { get; set; }
        public string longInPosition { get; set; }
    }
}
