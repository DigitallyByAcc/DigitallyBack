namespace DigitalyAPI.Models.DTO
{
    public class ClientWithImpayesDto
    {
        public int IdClient { get; set; }
        public string NomClient { get; set; }
        public string PrenomClient { get; set; }
        public int NombreImpaye { get; set; } // Nouvelle propriété ajoutée
        public string RefClient { get; set; }  // Ajouter la référence du client ici

        public List<ImpayeDto> Impayes { get; set; }
    }
}
