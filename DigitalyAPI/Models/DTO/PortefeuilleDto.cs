using DigitalyAPI.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DigitalyAPI.Models.DTO
{
    public class PortefeuilleDto
    {
        public int IdPortefeuille { get; set; }
        public string nomPortefeuille { get; set; }

        public DateTime DateCreation { get; set; }

        public int nbreDossiers { get; set; }

        public TypePortefeuille typeportefeuille { get; set; }

        //public List<RecouvreurDto> Recouvreurs { get; set; } = new List<RecouvreurDto>();
        public List<int> RecouvreursIds { get; set; }


    }
}
