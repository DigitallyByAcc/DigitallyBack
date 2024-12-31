using DigitalyAPI.Models.DTO;

namespace DigitalyAPI.Services.IService
{
    public interface IPortefeuilleService
    {
        Task<bool> AffectRecouvreursToPortefeuille(int portefeuilleId, List<int> recouvreursIds);
        Task<PortefeuilleDto> CreatePortefeuilleAndAssignRecouvreurs(PortefeuilleDto request);

    }
}
