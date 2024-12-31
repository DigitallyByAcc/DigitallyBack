using DigitalyAPI.Data;
using DigitalyAPI.Models.Domain;
using DigitalyAPI.Models.DTO;
using DigitalyAPI.Repositories.Implementation;
using DigitalyAPI.Repositories.Interface;
using DigitalyAPI.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace DigitalyAPI.Services.Service
{
    public class PortefeuilleService : IPortefeuilleService
    {
        private readonly ApplicationDBContext _context;

        public PortefeuilleService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> AffectRecouvreursToPortefeuille(int portefeuilleId, List<int> recouvreursIds)
        {
            // Validate the portefeuille
            var portefeuille = await _context.portefeuilles
                .FirstOrDefaultAsync(p => p.IdPortefeuille == portefeuilleId);

            if (portefeuille == null)
            {
                throw new Exception("Portefeuille not found");
            }

            // Fetch the recouvreurs to be reassigned
            var recouvreurs = await _context.recouvreurs
                .Where(r => recouvreursIds.Contains(r.IdRecouvreur))
                .ToListAsync();

            if (recouvreurs.Count != recouvreursIds.Count)
            {
                throw new Exception("Some recouvreurs not found");
            }

            // Reassign each recouvreur to the specified portefeuille
            foreach (var recouvreur in recouvreurs)
            {
                recouvreur.PortefeuilleId = portefeuilleId; // Update the foreign key
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PortefeuilleDto> CreatePortefeuilleAndAssignRecouvreurs(PortefeuilleDto request)
        {
            // Création du portefeuille
            var portefeuille = new Portefeuille
            {
                nomPortefeuille = request.nomPortefeuille,
                DateCreation = request.DateCreation,
                typeportefeuille = request.typeportefeuille,
                nbreDossiers = request.nbreDossiers,
            };

            await _context.portefeuilles.AddAsync(portefeuille);
            await _context.SaveChangesAsync(); // Enregistrer pour générer l'ID du portefeuille

            if (request.RecouvreursIds != null && request.RecouvreursIds.Any())
            {
                // Récupérer les recouvreurs
                var recouvreurs = await _context.recouvreurs
                    .Where(r => request.RecouvreursIds.Contains(r.IdRecouvreur))
                    .ToListAsync();

                if (recouvreurs.Count != request.RecouvreursIds.Count)
                {
                    throw new Exception("Certains recouvreurs ne sont pas trouvés.");
                }

                // Associer les recouvreurs au portefeuille
                portefeuille.Recouvreurs = recouvreurs;
            }

            // Enregistrer les modifications
            await _context.SaveChangesAsync();

            // Retourner le portefeuille créé avec ses recouvreurs
            return new PortefeuilleDto
            {
                IdPortefeuille = portefeuille.IdPortefeuille,
                nomPortefeuille = portefeuille.nomPortefeuille,
                DateCreation = portefeuille.DateCreation,
                typeportefeuille = portefeuille.typeportefeuille,
                nbreDossiers = portefeuille.nbreDossiers,
                RecouvreursIds = portefeuille.Recouvreurs?.Select(r => r.IdRecouvreur).ToList() ?? new List<int>()
            };
        }


    }

}

