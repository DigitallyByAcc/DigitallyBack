using DigitalyAPI.Models.Domain;

namespace DigitalyAPI.Repositories.Interface
{
    public interface IImpayeRepository
    {
        Task<IEnumerable<Impaye>> GetAllImpayeAsync();
        Task<Impaye?> GetImpayeByIdAsync(int id);
        Task<Impaye> AddImpayeAsync(Impaye impaye);
        Task<Impaye?> UpdateImpayeAsync(int id, Impaye impaye);
        Task<bool> DeleteImpayeAsync(int id);
        Task<bool> AffecterImpayeAuPrestataire(int impayeId, int prestataireId);
        Task<int> GetNombreImpayesAsync(int clientId);
        Task<List<Impaye>> GetImpayesByPrestataireAsync(int prestataireId);


    }
}
