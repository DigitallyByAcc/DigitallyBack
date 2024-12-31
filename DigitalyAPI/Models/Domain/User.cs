using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalyAPI.Models.Domain
{
    public class User :IdentityUser
    {
        [Required]
        public string  FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public DateTime DateCreated { get; set; } =DateTime.Now;

        // public List<string> Roles { get; set; } // Doit être une liste ou un tableau

        public string RefreshToken { get; set; } = string.Empty; // Provide a default value
        public DateTime? RefreshTokenExpiryTime { get; set; }


        public string PhoneNumber { get; set; }

        public string PhoneFix  { get; set; } 
        public string Fax { get; set; }
        public string Civilite { get; set; }
        public string? Dateofbirth { get; set; }

        public string Address { get; set; } = string.Empty;
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Grade { get; set; }
        public string longInPosition { get; set; }
        
        

    }
}
