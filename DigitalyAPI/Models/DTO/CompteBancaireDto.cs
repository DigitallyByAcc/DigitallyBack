﻿namespace DigitalyAPI.Models.DTO
{
    public class CompteBancaireDto
    {
        public int Idcompte { get; set; }
        public string salairemensuel { get; set; }
        public string mnt_bloque { get; set; }
        public long rib { get; set; }
        public string segmentation { get; set; }
        public string engagement_total { get; set; }
        public string devisecompte { get; set; }
        public string soldedispo { get; set; }
        public string classe_risque { get; set; }
        public string tot_mvt_cred { get; set; }
        public long iban { get; set; }
        public string clientNom { get; set; } // Nom du client associé
       public string clientPrenom { get; set; } // Prénom du client associé
    }
}