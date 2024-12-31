using DigitalyAPI.Models.Domain;

namespace DigitalyAPI.Models.DTO
{
    public class ClientWithAccountDto
    {
        public int IdClient { get; set; }
        public string refclient { get; set; }
        public string Nom { get; set; }

        public string Prenom { get; set; }

        public string Nationnalite { get; set; }

        public string profession { get; set; }

        public string adresse { get; set; }



        public string email { get; set; }

        public string situationfamiliale { get; set; }

        public DateTime Datedenaiss { get; set; }

        public int telephone { get; set; }

        public string Fonction { get; set; }

        public string Anciennete { get; set; }
        public CompteBancaire compteBancaire { get; set; }

    }
}
