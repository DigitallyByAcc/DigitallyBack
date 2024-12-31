using DigitalyAPI.Models.Domain;

namespace DigitalyAPI.Repositories.Interface
{
    public interface IPrestataireRepository

    {
        Task<Prestataire> CreateAsync(Prestataire prestataire);
        Task<Prestataire> GetByIdAsync(int id);
        Task<IEnumerable<Prestataire>> GetAllAsync();  // Nouvelle méthode pour récupérer tous les prestataires
        Task<Prestataire> DeleteAsync(int id);
        Task<Prestataire> UpdateAsync(int id, Prestataire prestataire); // Nouvelle méthode pour mettre à jour un client

        Task<Prestataire?> GetLastPrestataireAsync();
        Task<Prestataire> GetByEmailAsync(string email);
        Task<List<Prestataire>> GetAllWithImpayesAsync();
        Task<IEnumerable<Impaye>> GetImpayesByPrestataireIdAsync(int prestataireId);
     //   Task<Prestataire> ArchiveAsync(int id);


    }
}
