namespace DigitalyAPI.Models.DTO
{
    public class ImpayeDto
    {
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
        public int? PrestataireId { get; set; }
    }
}
