using DigitalyAPI.Models.Domain;

namespace DigitalyAPI.Repositories.Interface
{
    public interface IPortefeuilleRepository
    {
        Task<Portefeuille> CreateAsync(Portefeuille portefeuille);
        Task<Portefeuille?> DeleteAsync(int id);
        Task<Portefeuille> GetByIdAsync(int id);
       // Task<Portefeuille> UpdateAsync(int id, Portefeuille portefeuille); // Nouvelle méthode pour mettre à jour un client

        Task<IEnumerable<Portefeuille>> GetAllAsync();  // Nouvelle méthode pour récupérer tous les clients

         Task<Portefeuille> GetPortefeuilleWithRecouvreursAsync(int portefeuilleId);

    }
}
