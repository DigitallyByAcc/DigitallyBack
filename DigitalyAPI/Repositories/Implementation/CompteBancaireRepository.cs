using DigitalyAPI.Data;
using DigitalyAPI.Models.Domain;
using DigitalyAPI.Models.DTO;
using DigitalyAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DigitalyAPI.Repositories.Implementation
{
    public class CompteBancaireRepository : ICompteBancaireRepository
    {
        private readonly ApplicationDBContext dBContext;

        public CompteBancaireRepository(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<CompteBancaire> CreateAsync(CompteBancaire compteBancaire)
        {
            await dBContext.comptesBancaires.AddAsync(compteBancaire);
            await dBContext.SaveChangesAsync();
            return compteBancaire;
        }

        public async Task<CompteBancaire> GetByIdAsync(int id)
        {
            return await dBContext.comptesBancaires
              .Include(c => c.Client)  // Exemple d'inclusion de la navigation
              .FirstOrDefaultAsync(c => c.Idcompte == id);
        }

        public async Task<CompteBancaire> UpdateAsync(int id, UpdateCompteBancaireDto updateRequest)
        {
            var existingCompte = await dBContext.comptesBancaires
                                                 .Include(cb => cb.Client)  // Include client if needed (for reference, but not for modification)
                                                 .FirstOrDefaultAsync(cb => cb.Idcompte == id);

            if (existingCompte == null) return null;

            // Update only the properties of the compte bancaire (not the client)
            existingCompte.salairemensuel = updateRequest.SalaireMensuel;
            existingCompte.mnt_bloque = updateRequest.MntBloque;
            existingCompte.rib = updateRequest.Rib;
            existingCompte.segmentation = updateRequest.Segmentation;
            existingCompte.engagement_total = updateRequest.EngagementTotal;
            existingCompte.devisecompte = updateRequest.DeviseCompte;
            existingCompte.soldedispo = updateRequest.SoldeDispo;
            existingCompte.classe_risque = updateRequest.ClasseRisque;
            existingCompte.tot_mvt_cred = updateRequest.TotMvtCred;
            existingCompte.iban = updateRequest.Iban;

            // ClientId is typically used to ensure the account is linked to a valid client, but do not modify client details
            existingCompte.ClientId = updateRequest.ClientId;

            await dBContext.SaveChangesAsync();
            return existingCompte;
        }


        public async Task<IEnumerable<CompteBancaire>> GetAllComptesAsync()
        {
            return await dBContext.comptesBancaires
                                   .Include(c => c.Client)  // Inclure les données du client associé
                                   .ToListAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var compte = await dBContext.comptesBancaires.FirstOrDefaultAsync(cb => cb.Idcompte == id);
            if (compte == null) return false;

            dBContext.comptesBancaires.Remove(compte);
            await dBContext.SaveChangesAsync();
            return true;
        }


    }
}
