using DigitalyAPI.Data;
using DigitalyAPI.Models.Domain;
using DigitalyAPI.Models.DTO;
using DigitalyAPI.Repositories.Implementation;
using DigitalyAPI.Repositories.Interface;
using DigitalyAPI.Services.IService;
using DigitalyAPI.Services.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortefeuillesController : ControllerBase
    {
        private readonly IPortefeuilleRepository portefeuilleRepository;
        private readonly IPortefeuilleService portefeuilleService;
        private readonly ApplicationDBContext dbcontext;

        public PortefeuillesController(IPortefeuilleRepository portefeuilleRepository, IPortefeuilleService portefeuilleService ,ApplicationDBContext dBContext)
        {

            this.portefeuilleRepository = portefeuilleRepository;
             this.portefeuilleService= portefeuilleService;
            this.dbcontext= dBContext;
        }


        [HttpPost("create-portefeuille")]
        public async Task<IActionResult> CreatePortefeuille(PortefeuilleCreateResponseDto request)
        {
            // Mapper le DTO au modèle de domaine
            var portefeuille = new Portefeuille
            {
                nomPortefeuille = request.nomPortefeuille,
                DateCreation = request.DateCreation,
                typeportefeuille = request.typeportefeuille,
                nbreDossiers = request.nbreDossiers,

              
            };

            // Ajouter le portefeuille via le repository
            await portefeuilleRepository.CreateAsync(portefeuille);

            // Mapper le modèle de domaine au DTO pour la réponse
            var response = new PortefeuilleDto
            {
                IdPortefeuille = portefeuille.IdPortefeuille,
                nomPortefeuille = portefeuille.nomPortefeuille,
                DateCreation = portefeuille.DateCreation,
                typeportefeuille = portefeuille.typeportefeuille,
                nbreDossiers = portefeuille.nbreDossiers,

               
            };

            return Ok(response);
        }

        [HttpDelete("delete-portefeuille/{id}")]
        public async Task<IActionResult> DeletePortefeuille(int id)
        {
            var deletedPortefeuille = await portefeuilleRepository.DeleteAsync(id);

            if (deletedPortefeuille == null)
            {
                return NotFound("Portefeuille not found");
            }

            return Ok("Portefeuille deleted and associated recouvreurs updated to PortefeuilleId = 0");
        }

        // récupérer tous les portefeuilles
        [HttpGet("get-all-portefeuilles")]
        public async Task<IActionResult> GetAllPortefeuilles()
        {
            // Récupérer les portefeuilles avec les clients associés
            var portefeuilles = await dbcontext.portefeuilles
                .Include(p => p.Clients) // Inclure les clients associés
                .ToListAsync();

            // Mapper les portefeuilles en DTOs et calculer le nombre de clients
            var listportefeuilles = portefeuilles.Select(portefeuille => new PortefeuilleDto
            {
                IdPortefeuille = portefeuille.IdPortefeuille,
                nomPortefeuille = portefeuille.nomPortefeuille,
                DateCreation = portefeuille.DateCreation,
                nbreDossiers = portefeuille.Clients.Count(), // Nombre de clients dans le portefeuille
                typeportefeuille = portefeuille.typeportefeuille // Convertir la valeur enum en chaîne
            }).ToList();

            return Ok(listportefeuilles);
        }
        [HttpGet("get-portefeuille/{id}")]
        public async Task<IActionResult> GetPortefeuilleById(int id)
        {
            var portefeuille = await portefeuilleRepository.GetByIdAsync(id);

            if (portefeuille == null)
            {
                return NotFound();
            }

            // Mapper le modèle de domaine au DTO
            var portefeuilleDtoo = new PortefeuilleDto
            {
                IdPortefeuille = portefeuille.IdPortefeuille,
                nomPortefeuille=portefeuille.nomPortefeuille,
                DateCreation=portefeuille.DateCreation,  
                nbreDossiers=portefeuille.nbreDossiers,
                typeportefeuille = portefeuille.typeportefeuille
            };

            return Ok(portefeuilleDtoo);
        }




        /*  [HttpPut("update-portefeuille/{id}")]
          public async Task<IActionResult> UpdatePortefeuille(int id, PortefeuilleDto request)
          {
              var portefeuille = new Portefeuille
              {
                  RefClient = request.RefClient,
                  nomPortefeuille=request.nomPortefeuille,
                  Retard = request.Retard,
                  DateCreation = request.DateCreation,
                  DateEcheance = request.DateEcheance,
                  Priorite = request.Priorite,
                  MontantImpaye = request.MontantImpaye,
                  typeportefeuille = request.TypePortefeuille
              };

              var updatedPortefeuille = await portefeuilleRepository.UpdateAsync(id, portefeuille);

              if (updatedPortefeuille == null)
              {
                  return NotFound();
              }

              var response = new PortefeuilleDto
              {
                  IdPortefeuille = updatedPortefeuille.IdPortefeuille,
                  nomPortefeuille=updatedPortefeuille.nomPortefeuille,
                  RefClient = updatedPortefeuille.RefClient,
                  Retard = updatedPortefeuille.Retard,
                  DateCreation = updatedPortefeuille.DateCreation,
                  DateEcheance = updatedPortefeuille.DateEcheance,
                  Priorite = updatedPortefeuille.Priorite,
                  MontantImpaye = updatedPortefeuille.MontantImpaye,
                  TypePortefeuille = updatedPortefeuille.typeportefeuille,
              };

              return Ok(response);
          }*/


        [HttpPost("affect-recouvreurs")]
        public async Task<IActionResult> AffectRecouvreursToPortefeuille(AffectRecouvreursToPortefeuilleDto request)
        {
            try
            {
                var success = await portefeuilleService.AffectRecouvreursToPortefeuille (request.PortefeuilleId, request.RecouvreursIds);

                if (success)
                {
                    return Ok("Recouvreurs successfully assigned to portefeuille");
                }
                else
                {
                    return BadRequest("Failed to assign recouvreurs to portefeuille");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-recouvreurs-by-portefeuille/{id}")]
        public async Task<IActionResult> GetRecouvreursByPortefeuille(int id)
        {
            var portefeuille = await portefeuilleRepository.GetPortefeuilleWithRecouvreursAsync(id);

            if (portefeuille == null)
            {
                return NotFound("Portefeuille introuvable");
            }

            var recouvreurs = portefeuille.Recouvreurs.Select(r => new RecouvreurDto
            {
                IdRecouvreur = r.IdRecouvreur,
                nom = r.nom,
                prenom = r.prenom,
                email = r.email,
                mobile = r.mobile,
            }).ToList();

            return Ok(recouvreurs);
        }


        [HttpPost("create-and-assign")]
        public async Task<IActionResult> CreatePortefeuilleAndAssignRecouvreurs([FromBody] PortefeuilleDto request)
        {
            try
            {
                // Appeler la méthode combinée du service
                var response = await portefeuilleService.CreatePortefeuilleAndAssignRecouvreurs(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
