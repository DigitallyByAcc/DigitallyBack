using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;


namespace DigitalyAPI.Models.Domain
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum TypePortefeuille
    {
        [EnumMember(Value = "Aimable")]
        Aimable,
        [EnumMember(Value = "Contentieux")]
        Contentieux
    }
    public class Portefeuille
    {

        [Key]
        public int IdPortefeuille { get; set; }
        public string nomPortefeuille { get;set; }
        
        public DateTime DateCreation { get; set; }

        public int nbreDossiers { get; set; }

        [EnumDataType(typeof(TypePortefeuille))]
        public TypePortefeuille typeportefeuille { get; set; }

        // Relation avec Recouvreurs
        public ICollection<Recouvreur> Recouvreurs { get; set; } = new List<Recouvreur>();


        // relation avec la classe imapye
        //  public ICollection<Impaye> Impayes { get; set; }
        // Relation un-à-plusieurs avec les clients
        public ICollection<Client> Clients { get; set; } = new List<Client>();


    }
}
