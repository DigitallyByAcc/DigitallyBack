using DigitalyAPI.Data;
using DigitalyAPI.Models.Domain;
using DigitalyAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DigitalyAPI.Repositories.Implementation
{
    public class ImpayeRepository : IImpayeRepository
    {
        private readonly ApplicationDBContext _context;

        public ImpayeRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Impaye> AddImpayeAsync(Impaye impaye)
        {
            _context.impayes.Add(impaye);
            await _context.SaveChangesAsync();
            return impaye;
           
        }

            public async  Task<bool> DeleteImpayeAsync(int id)
        {
            var impaye = await _context.impayes.FindAsync(id);
            if (impaye == null) return false;

            _context.impayes.Remove(impaye);
            await _context.SaveChangesAsync();
            return true;
        }

        public async  Task<IEnumerable<Impaye>> GetAllImpayeAsync()
        {
            return await _context.impayes.Include(i => i.Client).ToListAsync();
        }

        public async  Task<Impaye?> GetImpayeByIdAsync(int id)
        {
            return await _context.impayes.Include(i => i.Client).FirstOrDefaultAsync(i => i.Idimpaye == id);
        }

        public async  Task<Impaye?> UpdateImpayeAsync(int id, Impaye impaye)
        {
            var existingImpaye = await _context.impayes.FindAsync(id);
            if (existingImpaye == null) return null;

            existingImpaye.ref_impaye = impaye.ref_impaye;
            existingImpaye.typeimpaye = impaye.typeimpaye;
            existingImpaye.date_impaye = impaye.date_impaye;
            existingImpaye.echeance_Principale = impaye.echeance_Principale;
            existingImpaye.retard = impaye.retard;
            existingImpaye.type_credit = impaye.type_credit;
            existingImpaye.mt_total = impaye.mt_total;
            existingImpaye.mt_impaye = impaye.mt_impaye;
            existingImpaye.mt_encours = impaye.mt_encours;
            existingImpaye.interet = impaye.interet;
            existingImpaye.statut = impaye.statut;
            existingImpaye.ClientId = impaye.ClientId;

            await _context.SaveChangesAsync();
            return existingImpaye;
        }
        public async Task<bool> AffecterImpayeAuPrestataire(int impayeId, int prestataireId)
        {
            var impaye = await _context.impayes.FindAsync(impayeId);
            var prestataire = await _context.prestataires.FindAsync(prestataireId);

            if (impaye == null || prestataire == null)
                return false;

            impaye.PrestataireId = prestataireId; // Assurez-vous que la propriété `PrestataireId` existe dans la classe `Impaye`.
            await _context.SaveChangesAsync();

            return true;
        }

        // Méthode pour obtenir le nombre d'impayés pour un client donné
        public async Task<int> GetNombreImpayesAsync(int clientId)
        {
            return await _context.impayes
                                 .Where(i => i.ClientId == clientId)
                                 .CountAsync();
        }

        public async Task<List<Impaye>> GetImpayesByPrestataireAsync(int prestataireId)
        {
            return await _context.impayes
                .Include(i => i.Client) // Inclure les détails du client
                .Where(i => i.PrestataireId == prestataireId) // Filtrer par prestataire
                .ToListAsync();
        }
    }
}
