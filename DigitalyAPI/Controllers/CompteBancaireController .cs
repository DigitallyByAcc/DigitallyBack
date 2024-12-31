using DigitalyAPI.Data;
using DigitalyAPI.Models.Domain;
using DigitalyAPI.Models.DTO;
using DigitalyAPI.Repositories.Implementation;
using DigitalyAPI.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompteBancaireController : ControllerBase
    {
        private readonly ICompteBancaireRepository _compteBancaireRepository;
        private readonly IClientRepository clientRepository;
        private readonly ApplicationDBContext _dbContext;

        public CompteBancaireController(ICompteBancaireRepository compteBancaireRepository, IClientRepository clientRepository,ApplicationDBContext dBContext)
        {
            _compteBancaireRepository = compteBancaireRepository;
            this.clientRepository = clientRepository;
            this.clientRepository = clientRepository;
            _dbContext = dBContext;

        }

        // Ajouter un compte bancaire pour un client
        /*  [HttpPost("create-compte/{clientId}")]
          public async Task<IActionResult> CreateCompte(int clientId, [FromBody] CompteBancaire compteBancaire)
          {
              // Vérifier si le client existe
              var client = await clientRepository.GetByIdAsync(clientId);
              if (client == null)
              {
                  return NotFound("Client not found");
              }

              // Associer le compte au client
              compteBancaire.client = client;

              var newCompte = await _compteBancaireRepository.CreateAsync(compteBancaire);
              return CreatedAtAction(nameof(GetCompteById), new { id = newCompte.Idcompte }, newCompte);
          }*/
        [HttpPost("create-compte/{clientId}")]
        public async Task<IActionResult> CreateCompte(int clientId, [FromBody] CompteBancaireCreateDto compteBancaireDto)
        {
            // Vérifier si le client existe
            var client = await clientRepository.GetByIdAsync(clientId);
            if (client == null)
            {
                return NotFound("Client not found");
            }

            // Créer un nouvel objet CompteBancaire avec les données du DTO
            var compteBancaire = new CompteBancaire
            {
                salairemensuel = compteBancaireDto.salairemensuel,
                mnt_bloque = compteBancaireDto.mnt_bloque,
                rib = compteBancaireDto.rib,
                segmentation = compteBancaireDto.segmentation,
                engagement_total = compteBancaireDto.engagement_total,
                devisecompte = compteBancaireDto.devisecompte,
                soldedispo = compteBancaireDto.soldedispo,
                classe_risque = compteBancaireDto.classe_risque,
                tot_mvt_cred = compteBancaireDto.tot_mvt_cred,
                iban = compteBancaireDto.iban,
                ClientId = clientId // Associer l'ID du client
            };

            // Créer le compte bancaire dans la base de données
            var newCompte = await _compteBancaireRepository.CreateAsync(compteBancaire);

            // Retourner la réponse avec le compte bancaire créé
            return CreatedAtAction(nameof(GetCompteById), new { id = newCompte.Idcompte }, newCompte);
        }




        // Récupérer un compte bancaire par ID
        [HttpGet("get-compte/{id}")]
        public async Task<IActionResult> GetCompteById(int id)
        {
            var compte = await _dbContext.comptesBancaires
            .Include(c => c.Client)  // Inclure le client associé
            .FirstOrDefaultAsync(c => c.Idcompte == id);

            if (compte == null)
            {
                return NotFound("Compte bancaire not found");
            }

            return Ok(compte);
        }
        [HttpPut("update-compte/{id}")]
        public async Task<IActionResult> UpdateCompte(int id, [FromBody] UpdateCompteBancaireRequest updateRequest)
        {
            // Validate the request
            if (updateRequest == null || updateRequest.UpdateRequest == null)
            {
                return BadRequest("Invalid update request.");
            }

            var updatedCompte = await _compteBancaireRepository.UpdateAsync(id, updateRequest.UpdateRequest);

            if (updatedCompte == null)
            {
                return NotFound("Compte bancaire not found");
            }

            return Ok(updatedCompte);
        }

        public class UpdateCompteBancaireRequest
        {
            public UpdateCompteBancaireDto UpdateRequest { get; set; }
        }

        // Récupérer tous les comptes bancaires de tous les clients
        [HttpGet("get-all-comptes")]
        public async Task<IActionResult> GetAllComptes()
        {
            var comptes = await _compteBancaireRepository.GetAllComptesAsync();

            if (comptes == null || !comptes.Any())
            {
                return NotFound("No comptes found");
            }

            // Mapper les comptes bancaires en DTO ou les renvoyer tels quels
            var comptesDto = comptes.Select(compte => new CompteBancaireDto
            {
                Idcompte = compte.Idcompte,
                salairemensuel = compte.salairemensuel,
                mnt_bloque = compte.mnt_bloque,
                rib = compte.rib,
                segmentation = compte.segmentation,
                engagement_total = compte.engagement_total,
                devisecompte = compte.devisecompte,
                soldedispo = compte.soldedispo,
                classe_risque = compte.classe_risque,
                tot_mvt_cred = compte.tot_mvt_cred,
                iban = compte.iban,
                clientNom = compte.Client?.Nom,  // Accéder aux informations du client
                clientPrenom = compte.Client?.Prenom
            }).ToList();

            return Ok(comptesDto);

        }

        // Supprimer un compte bancaire
        [HttpDelete("delete-compte/{id}")]
        public async Task<IActionResult> DeleteCompte(int id)
        {
            var result = await _compteBancaireRepository.DeleteAsync(id);

            if (!result)
            {
                return NotFound("Compte bancaire not found");
            }

            return NoContent();
        }


    }
}
