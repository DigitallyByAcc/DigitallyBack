using DigitalyAPI.Models.Domain;

namespace DigitalyAPI.Models.DTO
{

    public class PrestataireDto
    {
        public int IdPrestataire { get; set; }
        public string? refprestataire { get; set; }
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


        public int nombre_dossiers_encharge { get; set; }
        public string zone_geo { get; set; }
        public rolepres role { get; set; }

        public string status { get; set; }

        public string codepostal { get; set; }
        public string? retard_paiement { get; set; } // Montant total des impayés


    }
}
