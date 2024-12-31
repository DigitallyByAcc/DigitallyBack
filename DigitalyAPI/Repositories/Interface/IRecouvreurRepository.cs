using DigitalyAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitalyAPI.Repositories.Interface
{
    public interface IRecouvreurRepository
    {
        Task<Recouvreur> CreateAsync(Recouvreur recouvreur);
        Task<IEnumerable<Recouvreur>> GetAllAsync();  // Nouvelle méthode pour récupérer tous les recouvreurs
        Task<Recouvreur> GetByIdAsync(int id);

        Task<Recouvreur> UpdateAsync(int id, Recouvreur recouvreur); // Nouvelle méthode pour mettre à jour un client
        Task<Recouvreur> DeleteAsync(int id);
        Task<List<Recouvreur>> GetRecouvreursNonAffectesAsync();
        Task<Recouvreur?> GetLastRecouvreurAsync();
        Task<Recouvreur> GetByEmailAsync(string email);
        Task<List<Recouvreur>> GetRecouvreursWithClientImpayesAsync();
        Task<Recouvreur?> GetRecouvreurInformationByIdAsync(int idRecouvreur);
        Task<Recouvreur> GetByUserIdAsync(string userId);
        Task<List<Impaye>> GetImpayesByRecouvreurAsync(int recouvreurId);
       




    }
}
