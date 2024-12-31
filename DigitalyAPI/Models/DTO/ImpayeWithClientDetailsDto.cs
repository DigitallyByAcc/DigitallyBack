namespace DigitalyAPI.Models.DTO
{
    public class ImpayeWithClientDetailsDto
    {
        // Informations sur l'impayé
        public int Idimpaye { get; set; }
        public string ref_impaye { get; set; }
        public string typeimpaye { get; set; }
        public DateTime date_impaye { get; set; }
        public DateTime echeance_Principale { get; set; }
        public string retard { get; set; }
        public string type_credit { get; set; }
        public string mt_total { get; set; }
        public string mt_impaye { get; set; }
        public string mt_encours { get; set; }
        public string interet { get; set; }
        public string statut { get; set; }

        // Détails du client
        public int IdClient { get; set; }
        public string refclient { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string email { get; set; }
        public string telephone { get; set; }
    }
}
