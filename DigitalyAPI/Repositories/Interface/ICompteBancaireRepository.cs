using DigitalyAPI.Models.Domain;
using DigitalyAPI.Models.DTO;

namespace DigitalyAPI.Repositories.Interface
{
    public interface ICompteBancaireRepository
    {
        Task<CompteBancaire> CreateAsync(CompteBancaire compteBancaire);
        Task<CompteBancaire> GetByIdAsync(int id);
        Task<IEnumerable<CompteBancaire>> GetAllComptesAsync();

        Task<CompteBancaire> UpdateAsync(int id, UpdateCompteBancaireDto updateRequest);
        Task<bool> DeleteAsync(int id);
    }
}
