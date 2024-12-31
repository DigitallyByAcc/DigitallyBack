using DigitalyAPI.Data;
using DigitalyAPI.Models.Domain;
using DigitalyAPI.Models.DTO;
using DigitalyAPI.Repositories.Implementation;
using DigitalyAPI.Repositories.Interface;
using DigitalyAPI.Services.IService;

namespace DigitalyAPI.Services.Service
{
    public class RecouvreurService: IRecouvreurService
    {
        private readonly ApplicationDBContext _context;
        private readonly IRecouvreurRepository _recouvreurRepository;


        public RecouvreurService(ApplicationDBContext context, IRecouvreurRepository recouvreurRepository)
        {
            _context = context;
            _recouvreurRepository = recouvreurRepository;

        }



        public async Task<List<RecouvreurDto>> GetRecouvreursWithClientImpayesAsync()
        {
            var recouvreurs = await _recouvreurRepository.GetRecouvreursWithClientImpayesAsync();

            return recouvreurs.Select(r => new RecouvreurDto
            {
                IdRecouvreur = r.IdRecouvreur,
                nom = r.nom,
                prenom = r.prenom,
                NombreDossier = r.Clients.Count, // Nombre de clients associés
                PortefeuilleId = r.PortefeuilleId, // ID du portefeuille
                Clients = r.Clients.Select(c => new ClientWithImpayesDto
                {
                    IdClient = c.IdClient,
                    NomClient = c.Nom,
                    PrenomClient = c.Prenom,
                    Impayes = c.impayes.Select(i => new ImpayeDto
                    {
                        Idimpaye = i.Idimpaye,
                        ref_impaye = i.ref_impaye,
                        typeimpaye = i.typeimpaye,
                        date_impaye = i.date_impaye,
                        echeance_Principale = i.echeance_Principale,
                        retard = i.retard,
                        mt_total = i.mt_total,
                        mt_impaye = i.mt_impaye,
                        mt_encours = i.mt_encours,
                        interet = i.interet,
                        statut = i.statut
                    }).ToList()
                }).ToList()
            }).ToList();
        }
        public async Task<RecouvreurDto?> GetRecouvreurInformationByIdAsync(int idRecouvreur)
        {
            var recouvreur = await _recouvreurRepository.GetRecouvreurInformationByIdAsync(idRecouvreur);

            if (recouvreur == null)
                return null;

            return new RecouvreurDto
            {
                IdRecouvreur = recouvreur.IdRecouvreur,
                nom = recouvreur.nom,
                prenom = recouvreur.prenom,
                NombreDossier = recouvreur.Clients.Count,
                PortefeuilleId = recouvreur.PortefeuilleId,
                Clients = recouvreur.Clients.Select(c => new ClientWithImpayesDto
                {
                    IdClient = c.IdClient,
                    NomClient = c.Nom,
                    PrenomClient = c.Prenom,
                    NombreImpaye = c.impayes.Count,
                    RefClient = c.refclient,  // Ajout de la référence du client ici
                    Impayes = c.impayes.Select(i => new ImpayeDto
                    {
                        Idimpaye = i.Idimpaye,
                        ref_impaye = i.ref_impaye,
                        typeimpaye = i.typeimpaye,
                        date_impaye = i.date_impaye,
                        echeance_Principale = i.echeance_Principale,
                        retard = i.retard,
                        mt_total = i.mt_total,
                        mt_impaye = i.mt_impaye,
                        mt_encours = i.mt_encours,
                        interet = i.interet,
                        statut = i.statut
                    }).ToList()
                }).ToList()
            };
        }

        public async Task<List<Impaye>> GetImpayesByRecouvreurAsync(int recouvreurId)
        {
            return await _recouvreurRepository.GetImpayesByRecouvreurAsync(recouvreurId);
        }



    }
}

