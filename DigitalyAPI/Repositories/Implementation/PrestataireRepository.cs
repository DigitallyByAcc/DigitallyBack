using DigitalyAPI.Data;
using DigitalyAPI.Models.Domain;
using DigitalyAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DigitalyAPI.Repositories.Implementation
{
    public class PrestataireRepository:IPrestataireRepository
    {
       
        private readonly ApplicationDBContext dBContext;

        public PrestataireRepository(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<Prestataire> CreateAsync(Prestataire prestataire)
        {
            await dBContext.prestataires.AddAsync(prestataire);
            await dBContext.SaveChangesAsync();

            return prestataire;
        }

        public async Task<Prestataire> DeleteAsync(int id)
        {
            var prestataire = await dBContext.prestataires.FirstOrDefaultAsync(p => p.IdPrestataire == id);

            if (prestataire == null)
            {
                return null;
            }

            dBContext.prestataires.Remove(prestataire);
            await dBContext.SaveChangesAsync();
            return prestataire;
        }

        public async Task<IEnumerable<Prestataire>> GetAllAsync()
        {
            return await dBContext.prestataires.ToListAsync();
        }

        public async Task<Prestataire> GetByIdAsync(int id)
        {
            return await dBContext.prestataires
                .Include(p => p.Impayes) // Inclure les impayés associés
                .FirstOrDefaultAsync(p => p.IdPrestataire == id);
        }



        public async Task<Prestataire> UpdateAsync(int id, Prestataire prestataire)
        {
            // Recherche du prestataire existant dans la base de données
            var existingPrestataire = await dBContext.prestataires.FirstOrDefaultAsync(p => p.IdPrestataire == id);

            // Vérifie si le prestataire existe
            if (existingPrestataire == null)
            {
                return null;
            }

            // Mise à jour des propriétés du prestataire existant avec les nouvelles valeurs
            existingPrestataire.Profile = prestataire.Profile;
            existingPrestataire.civilite = prestataire.civilite;
            existingPrestataire.email = prestataire.email;
            existingPrestataire.nom = prestataire.nom;
            existingPrestataire.prenom = prestataire.prenom;
            existingPrestataire.mobile = prestataire.mobile;
            existingPrestataire.fax = prestataire.fax;
            existingPrestataire.adresse = prestataire.adresse;
            existingPrestataire.agence = prestataire.agence;
            existingPrestataire.pays = prestataire.pays;
            existingPrestataire.ville = prestataire.ville;
            existingPrestataire.status = prestataire.status;
            existingPrestataire.codepostal = prestataire.codepostal;
            existingPrestataire.zone_geo = prestataire.zone_geo;
            existingPrestataire.role = prestataire.role;
            existingPrestataire.refprestataire = prestataire.refprestataire;

            // Enregistrement des modifications dans la base de données
            await dBContext.SaveChangesAsync();

            // Retourne le prestataire mis à jour
            return existingPrestataire;
        }

        public async Task<Prestataire?> GetLastPrestataireAsync()
        {
            return await dBContext.prestataires
                .OrderByDescending(r => r.IdPrestataire) // Assuming IdRecouvreur is sequential
                .FirstOrDefaultAsync();
        }
        public async Task<Prestataire> GetByEmailAsync(string email)
        {
            return await dBContext.prestataires
                                 .FirstOrDefaultAsync(r => r.email == email);
        }
        public async Task<List<Prestataire>> GetAllWithImpayesAsync()
        {
            return await dBContext.prestataires
                .Include(p => p.Impayes) // Inclure la collection des impayés
                .ToListAsync();
        }
        public async Task<IEnumerable<Impaye>> GetImpayesByPrestataireIdAsync(int prestataireId)
        {
            return await dBContext.impayes
                .Where(i => i.PrestataireId == prestataireId)
                .ToListAsync();
        }

      /*  public async Task<Prestataire> ArchiveAsync(int id)
        {
            var prestataire = await dBContext.prestataires.FirstOrDefaultAsync(p => p.IdPrestataire == id);

            if (prestataire == null)
            {
                return null; // Prestataire non trouvé
            }

            // Marquer le prestataire comme archivé
            prestataire.IsArchived = true;
            await dBContext.SaveChangesAsync();

            return prestataire; // Retourner l'objet archivé
        }*/

    }
}
    

