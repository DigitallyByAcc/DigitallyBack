using System.ComponentModel.DataAnnotations;

namespace DigitalyAPI.Models.Domain
{
    public class Contrat
    {
        [Key]
        public int Id_Contrat { get; set; }
        public string mont_total {  get; set; }
    }
}
