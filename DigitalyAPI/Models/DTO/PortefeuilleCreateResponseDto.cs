using DigitalyAPI.Models.Domain;

namespace DigitalyAPI.Models.DTO
{
    public class PortefeuilleCreateResponseDto
    {
        public int IdPortefeuille { get; set; }
        public string nomPortefeuille { get; set; }

        public DateTime DateCreation { get; set; }

        public int nbreDossiers { get; set; }

        public TypePortefeuille typeportefeuille { get; set; }
    }
}
