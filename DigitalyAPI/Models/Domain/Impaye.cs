using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalyAPI.Models.Domain
{
    public class Impaye
    {
        [Key]
        public int Idimpaye { get; set; }

        public string ref_impaye { get; set; }


        public string typeimpaye { get; set; }

        public DateTime date_impaye { get; set;}

        public DateTime echeance_Principale { get; set; }

        public string retard { get; set; }

        public string type_credit { get; set; }

        public string mt_total { get; set; }

        public string mt_impaye { get; set; }

        public string mt_encours { get; set; }

        public string interet { get; set; }

        public string statut { get; set; }



        //  public virtual Portefeuille Portefeuille { get; set; }
        // Foreign key
        //  [ForeignKey("Portefeuille")]
        //public int PortefeuilleFK { get; set; }

        // Clé étrangère optionnelle : relation impayes avec client 
        public int? ClientId { get; set; }

        public Client? Client { get; set; }
        // Foreign key for Prestataire
        public int? PrestataireId { get; set; }
        public Prestataire? Prestataire { get; set; }

    }
}
