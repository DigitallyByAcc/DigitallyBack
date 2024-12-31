using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DigitalyAPI.Models.Domain
{
    public class Client
    {
        [Key]
        public int IdClient { get; set; }
         public string refclient { get; set; }
        public string Nom { get; set; }

        public string Prenom { get; set; }

        public string Nationnalite { get; set; }

        public string profession { get; set; }

        public string adresse { get; set; }

       

        public string email { get; set; }

        public string situationfamiliale { get; set; }

        public DateTime Datedenaiss { get; set; }

        public int telephone { get; set; }

        public string Fonction { get; set; }

        public string Anciennete { get; set; }



        //relation one to one avec compte bancaire
       // [JsonIgnore]
        public CompteBancaire? compteBancaire { get; set; }

        // Relationone to many avec impayes
       // [JsonIgnore]

        public ICollection<Impaye> impayes { get; set; } = new List<Impaye>();

        // Relation un-à-plusieurs avec Portefeuille
        //[ForeignKey("Portefeuille")]
        public int? PortefeuilleId { get; set; }
        public Portefeuille? Portefeuille { get; set; } // Propriété de navigation


        // Relation avec le Recouvreur 
        public int? RecouvreurId { get; set; } // Clé étrangère facultative
        public Recouvreur? Recouvreur { get; set; } // Navigation Property


    }
}
