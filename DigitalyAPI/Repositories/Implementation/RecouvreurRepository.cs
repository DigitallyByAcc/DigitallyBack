using DigitalyAPI.Data;
using DigitalyAPI.Models.Domain;
using DigitalyAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DigitalyAPI.Repositories.Implementation
{
    public class RecouvreurRepository:IRecouvreurRepository
    {
        private readonly ApplicationDBContext dBContext;

        public RecouvreurRepository(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<Recouvreur> CreateAsync(Recouvreur recouvreur)
        {
            await dBContext.recouvreurs.AddAsync(recouvreur);
            await dBContext.SaveChangesAsync();

            return recouvreur;
        }

        public async Task<Recouvreur> DeleteAsync(int id)
        {
            var recouvreur = await dBContext.Set<Recouvreur>().FirstOrDefaultAsync(r => r.IdRecouvreur == id);

            if (recouvreur == null)
            {
                return null;
            }

            dBContext.Set<Recouvreur>().Remove(recouvreur);
            await dBContext.SaveChangesAsync();
            return recouvreur;
        }

        public async Task<IEnumerable<Recouvreur>> GetAllAsync()
        {
            return await dBContext.recouvreurs.ToListAsync();
        }

        public async Task<Recouvreur> GetByIdAsync(int id)
        {
            return await dBContext.recouvreurs.FirstOrDefaultAsync(r => r.IdRecouvreur == id);

        }

        public async Task<List<Recouvreur>> GetRecouvreursNonAffectesAsync()
        {
            
                return await dBContext.recouvreurs
                    .Where(r => r.PortefeuilleId == null || r.PortefeuilleId == 0) // Condition pour recouvreurs non affectés
                    .ToListAsync();
            
        }

        public async Task<Recouvreur> UpdateAsync(int id, Recouvreur recouvreur)
        {
            var existingRecouvreur = await dBContext.recouvreurs.FirstOrDefaultAsync(r => r.IdRecouvreur == id);

            if (existingRecouvreur == null)
            {
                return null;
            }

            existingRecouvreur.Profile = recouvreur.Profile;
            existingRecouvreur.civilite = recouvreur.civilite;
            existingRecouvreur.email = recouvreur.email;
            existingRecouvreur.nom = recouvreur.nom;
            existingRecouvreur.prenom = recouvreur.prenom;
            existingRecouvreur.mobile = recouvreur.mobile;
            existingRecouvreur.fax = recouvreur.fax;
            existingRecouvreur.adresse = recouvreur.adresse;
            existingRecouvreur.pays = recouvreur.pays;
            existingRecouvreur.ville = recouvreur.ville;
            existingRecouvreur.fonction = recouvreur.fonction;
            existingRecouvreur.grade = recouvreur.grade;
            existingRecouvreur.anciennete = recouvreur.anciennete;
            existingRecouvreur.status = recouvreur.status;
            existingRecouvreur.codepostal = recouvreur.codepostal;

            await dBContext.SaveChangesAsync();
            return existingRecouvreur;
        }
        public async Task<Recouvreur?> GetLastRecouvreurAsync()
        {
            return await dBContext.recouvreurs
                .OrderByDescending(r => r.IdRecouvreur) // Assuming IdRecouvreur is sequential
                .FirstOrDefaultAsync();
        }
        public async Task<Recouvreur> GetByEmailAsync(string email)
        {
            return await dBContext.recouvreurs
                                 .FirstOrDefaultAsync(r => r.email == email);
        }

      
        public async Task<List<Recouvreur>> GetRecouvreursWithClientImpayesAsync()
        {
            return await dBContext.recouvreurs
                .Include(r => r.Clients)
                    .ThenInclude(c => c.impayes)
                .ToListAsync();
        }

        public async Task<Recouvreur?> GetRecouvreurInformationByIdAsync(int idRecouvreur)
        {
            return await dBContext.recouvreurs
                .Include(r => r.Clients)
                    .ThenInclude(c => c.impayes)
                .FirstOrDefaultAsync(r => r.IdRecouvreur == idRecouvreur);
        }

        public async Task<Recouvreur> GetByUserIdAsync(string userId)
        {
            return await dBContext.recouvreurs
                .Include(r => r.user) // Optional if you need user details
                .FirstOrDefaultAsync(r => r.UserId == userId);
        }

        // Récupérer les impayés d'un recouvreur donné
        public async Task<List<Impaye>> GetImpayesByRecouvreurAsync(int recouvreurId)
        {
            return await dBContext.impayes
                .Include(impaye => impaye.Client) // Inclure les informations du client
                .Where(impaye => impaye.Client.RecouvreurId == recouvreurId) // Filtrer par recouvreur
                .ToListAsync();
        }
    }
}
