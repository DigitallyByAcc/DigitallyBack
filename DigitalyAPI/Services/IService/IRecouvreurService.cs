using DigitalyAPI.Models.Domain;
using DigitalyAPI.Models.DTO;

namespace DigitalyAPI.Services.IService
{
    public interface IRecouvreurService
    {
        Task<List<RecouvreurDto>> GetRecouvreursWithClientImpayesAsync();
        Task<RecouvreurDto?> GetRecouvreurInformationByIdAsync(int idRecouvreur);
        Task<List<Impaye>> GetImpayesByRecouvreurAsync(int recouvreurId);

    }
}
