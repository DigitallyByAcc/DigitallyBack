using DigitalyAPI.Models.Domain;

namespace DigitalyAPI.Models.DTO
{
    public class UpdateRecouvreurRequestDTO
    {
        public string Profile { get; set; }
        public string civilite { get; set; }

        public string email { get; set; }

        public string nom { get; set; }
        public string prenom { get; set; }

        public string mobile { get; set; }

        public string fax { get; set; }

        public string adresse { get; set; }

        public string pays { get; set; }

        public string ville { get; set; }

        public string fonction { get; set; }

        public string grade { get; set; }

        public string anciennete { get; set; }

        public string status { get; set; }

        public string codepostal { get; set; }
        public rolerecouv role { get; set; }
        public string refrecouvreur { get; set; }


    }
}
