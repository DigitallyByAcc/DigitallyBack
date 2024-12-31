namespace DigitalyAPI.Models.DTO
{
    public class AssignClientsToRecouvreurDto
    {
        public int RecouvreurId { get; set; } // Identifiant du recouvreur
        public List<int> ClientIds { get; set; } // Liste des identifiants des clients
    }
}
