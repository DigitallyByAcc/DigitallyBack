using DigitalyAPI.Data;
using DigitalyAPI.Models.Domain;
using DigitalyAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DigitalyAPI.Repositories.Implementation
{

    public class PortefeuilleRepository : IPortefeuilleRepository
    {
        private readonly ApplicationDBContext dBContext;

        public PortefeuilleRepository(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
      

            public async Task<Portefeuille> CreateAsync(Portefeuille portefeuille)
            {
            await dBContext.portefeuilles.AddAsync(portefeuille);
            await dBContext.SaveChangesAsync();

            return portefeuille;

            }

        public async Task<Portefeuille?> DeleteAsync(int id)
        {
            // Rechercher le portefeuille à supprimer
            var portefeuille = await dBContext.portefeuilles
                .Include(p => p.Recouvreurs) // Inclure les recouvreurs associés
                .FirstOrDefaultAsync(p => p.IdPortefeuille == id);

            if (portefeuille == null)
            {
                return null; // Si introuvable
            }

            // Désactiver la contrainte de clé étrangère
            await dBContext.Database.ExecuteSqlRawAsync("ALTER TABLE recouvreurs NOCHECK CONSTRAINT FK_recouvreurs_portefeuilles_PortefeuilleId");

            // Mettre à jour les recouvreurs pour affecter PortefeuilleId = 0
            foreach (var recouvreur in portefeuille.Recouvreurs)
            {
                recouvreur.PortefeuilleId = 0; // Mettre PortefeuilleId à 0
            }

            // Sauvegarder les changements dans les recouvreurs
            await dBContext.SaveChangesAsync();

            // Réactiver la contrainte de clé étrangère
            await dBContext.Database.ExecuteSqlRawAsync("ALTER TABLE recouvreurs CHECK CONSTRAINT FK_recouvreurs_portefeuilles_PortefeuilleId");

            // Supprimer le portefeuille
            dBContext.portefeuilles.Remove(portefeuille);
            await dBContext.SaveChangesAsync();

            return portefeuille;
        }


        public async Task<IEnumerable<Portefeuille>> GetAllAsync()
        {
            return await dBContext.portefeuilles.ToListAsync();   
        }

        public async Task<Portefeuille> GetByIdAsync(int id)
        {
            return await dBContext.portefeuilles.FirstOrDefaultAsync(p => p.IdPortefeuille == id);
        }

        public async Task<Portefeuille> GetPortefeuilleWithRecouvreursAsync(int portefeuilleId)
        {
            return await dBContext.portefeuilles
                .Include(p => p.Recouvreurs) // Inclure les recouvreurs
                .FirstOrDefaultAsync(p => p.IdPortefeuille == portefeuilleId);
        }

        /* public async Task<Portefeuille> UpdateAsync(int id, Portefeuille portefeuille)
         {

                 var existingPortefeuille = await dBContext.portefeuilles.FirstOrDefaultAsync(p => p.IdPortefeuille == id);

                 if (existingPortefeuille == null)
                 {
                     return null;
                 }

                 existingPortefeuille.RefClient = portefeuille.RefClient;
                 existingPortefeuille.Retard = portefeuille.Retard;
                 existingPortefeuille.DateCreation = portefeuille.DateCreation;
                 existingPortefeuille.DateEcheance = portefeuille.DateEcheance;
                 existingPortefeuille.Priorite = portefeuille.Priorite;
                 existingPortefeuille.MontantImpaye = portefeuille.MontantImpaye;
                 existingPortefeuille.typeportefeuille = portefeuille.typeportefeuille;
             existingPortefeuille.nomPortefeuille=portefeuille.nomPortefeuille;

                 await dBContext.SaveChangesAsync();
                 return existingPortefeuille;
         }*/
    }
}
    

