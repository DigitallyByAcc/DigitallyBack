using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Reflection;

namespace DigitalyAPI.Models.Domain
{
    public enum rolerecouv
    {
        RecouvreurAimable ,
        RecouvreurContentieux 

    }
   
    public class Recouvreur
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int IdRecouvreur { get; set; }
        public string refrecouvreur { get; set; }

        public string Profile { get; set; }
        public string civilite { get; set; }

        public string email { get; set; }

        public string nom { get; set; }
        public string prenom { get; set; }

        public string mobile {  get; set; }

        public string fax { get; set; }

        public string adresse { get; set; }

        public string pays { get; set; }

        public string ville { get; set; }

        public string fonction { get; set; }

        public string grade { get; set; }

        public string anciennete { get; set; }

        public string status { get; set; }

        public string codepostal { get; set; }

        public rolerecouv role { get; set; }

        // Clé étrangère optionnelle
        public int? PortefeuilleId { get; set; }
        public Portefeuille? Portefeuille { get; set; }

        // ralation avec client 
        public ICollection<Client> Clients { get; set; } = new List<Client>();

        // Foreign key for User
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public User? user { get; set; }

    }
}
