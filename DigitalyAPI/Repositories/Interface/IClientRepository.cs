using DigitalyAPI.Models.Domain;

namespace DigitalyAPI.Repositories.Interface
{
    public interface IClientRepository
    {
        Task<Client> CreateAsync(Client client);
        Task<IEnumerable<Client>> GetAllAsync();  // Nouvelle méthode pour récupérer tous les clients
        Task<Client?> GetByIdAsync(int id);

        Task<Client?> UpdateAsync(int id, Client client); // Nouvelle méthode pour mettre à jour un client
        Task<bool> DeleteAsync(int id);

        Task<Client> GetClientWithImpayesAsync(int clientId);
        Task<IEnumerable<Client>> GetAllClientsWithImpayesAsync();
        Task<List<Client>> GetClientsWithBankAccountsAsync();
        Task<Client?> GetClientWithAccountByIdAsync(int id);
        Task<bool> AssignClientToPortefeuilleAsync(int clientId, int portefeuilleId);
        Task<IEnumerable<Client>> GetClientsWithImpayesByPortefeuilleAsync(int portefeuilleId);
        Task<bool> AssignClientsToRecouvreurAsync(List<int> clientIds, int recouvreurId);


    }
}
