using System.ComponentModel.DataAnnotations;

namespace DigitalyAPI.Models.DTO
{
    public class UserDto
    {

        public string UserId { get; set; } // Add this property

        public string FirstName { get; set; }
       
        public string LastName { get; set; }
        [Required]
        // [RegularExpression("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$", ErrorMessage =" Invalid email adress")]
        public string Email { get; set; }
        public string JWT { get; set; }

        public List<string> Roles { get; set; }  // Add this property
        public string RefreshToken { get; set; } // Add this property

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

        // public string Role { get; set; }
    }
}
