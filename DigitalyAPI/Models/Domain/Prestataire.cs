using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DigitalyAPI.Models.Domain
{
    public enum rolepres 
    {
        Avocat ,
        Recouvreur_externe ,
        Huissier_notaire ,
        societe_de_recouvrement 

    }

   
    public class Prestataire
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public int IdPrestataire { get; set; }

        public string refprestataire {  get; set; }
        public string anciennete { get; set; }
        public string Profile { get; set; }
        public string civilite { get; set; }

        public string email { get; set; }

        public string nom { get; set; }
        public string prenom { get; set; }

        public string mobile { get; set; }

        public string fax { get; set; }

        public string adresse { get; set; }
        public string agence { get; set; }
        public string pays { get; set; }

        public string ville { get; set; }

        public rolepres role {  get; set; } 

        public int nombre_dossiers_encharge {  get; set; }
        public string zone_geo { get; set; }
        public string status { get; set; }
      //  public bool IsArchived { get; set; } // Champ pour marquer le prestataire comme archivé

        public string codepostal { get; set; }
        // Navigation property for related Impayes
        public ICollection<Impaye> Impayes { get; set; } = new List<Impaye>();
    }
}
