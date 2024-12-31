namespace DigitalyAPI.Models.DTO
{
    public class UpdateCompteBancaireDto
    {
        public string SalaireMensuel { get; set; }
        public string MntBloque { get; set; }
        public int Rib { get; set; }
        public string Segmentation { get; set; }
        public string EngagementTotal { get; set; }
        public string DeviseCompte { get; set; }
        public string SoldeDispo { get; set; }
        public string ClasseRisque { get; set; }
        public string TotMvtCred { get; set; }
        public int Iban { get; set; }
        public int ClientId { get; set; } // Client Id to identify the client linked to the account.
    }
}
