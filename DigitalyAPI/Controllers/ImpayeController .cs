using DigitalyAPI.Models.Domain;
using DigitalyAPI.Models.DTO;
using DigitalyAPI.Repositories.Interface;
using DigitalyAPI.Services.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImpayeController : ControllerBase
    {
        private readonly IImpayeRepository _impayeRepository;
        private readonly ImpayeService _impayeService;

        public ImpayeController(IImpayeRepository impayeRepository, ImpayeService impayeService)
        {
            _impayeRepository = impayeRepository;
            _impayeService = impayeService;

        }


        [HttpGet("get-all-impayes")]
        public async Task<IActionResult> GetAllImpaye()
        {
            var impayes = await _impayeRepository.GetAllImpayeAsync();
            return Ok(impayes);
        }

        [HttpGet("get-impaye/{id}")]
        public async Task<IActionResult> GetImpayeById(int id)
        {
            var impaye = await _impayeRepository.GetImpayeByIdAsync(id);
            if (impaye == null)
            {
                return NotFound("Impaye not found");
            }
            return Ok(impaye);
        }

        [HttpPost("add-impaye")]
        public async Task<IActionResult> AddImpaye([FromBody] CreateImpayeRequestDto impayeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var impaye = new Impaye
            {
                ref_impaye = impayeDto.ref_impaye,
                typeimpaye = impayeDto.typeimpaye,
                date_impaye = impayeDto.date_impaye,
                echeance_Principale = impayeDto.echeance_Principale,
                retard = impayeDto.retard,
                type_credit = impayeDto.type_credit,
                mt_total = impayeDto.mt_total,
                mt_impaye = impayeDto.mt_impaye,
                mt_encours = impayeDto.mt_encours,
                interet = impayeDto.interet,
                statut = impayeDto.statut,
                ClientId = impayeDto.ClientId
            };

            var createdImpaye = await _impayeRepository.AddImpayeAsync(impaye);
            return CreatedAtAction(nameof(GetImpayeById), new { id = createdImpaye.Idimpaye }, createdImpaye);
        }

        [HttpPut("update-impaye/{id}")]
        public async Task<IActionResult> UpdateImpaye(int id, [FromBody] Impaye impaye)
        {
            var updatedImpaye = await _impayeRepository.UpdateImpayeAsync(id, impaye);
            if (updatedImpaye == null)
            {
                return NotFound("Impaye not found");
            }
            return Ok(updatedImpaye);
        }

        [HttpDelete("delete-impaye/{id}")]
        public async Task<IActionResult> DeleteImpaye(int id)
        {
            var result = await _impayeRepository.DeleteImpayeAsync(id);
            if (!result)
            {
                return NotFound("Impaye not found");
            }
            return NoContent();
        }

        [HttpPost("affecter-prestataire")]
        public async Task<IActionResult> AffecterImpayeAuPrestataire([FromBody] AffectationPresRequestDto request)
        {
            if (request == null || request.ImpayeId <= 0 || request.PrestataireId <= 0)
            {
                return BadRequest("Données invalides");
            }

            var success = await _impayeRepository.AffecterImpayeAuPrestataire(request.ImpayeId, request.PrestataireId);

            if (!success)
                return NotFound("Impayé ou prestataire introuvable");

            return Ok("Impayé affecté avec succès au prestataire");
        }

        [HttpGet("{clientId}/nbre-impayes")]
        public async Task<ActionResult> GetNombreImpayes(int clientId)
        {
            var nombreImpayes = await _impayeService.GetNombreImpayes(clientId);
            return Ok(new { nombreImpayes });
        }


        [HttpGet("getimapayesandclients-by-prestataire/{prestataireId}")]
        public async Task<IActionResult> GetImpayesByPrestataire(int prestataireId)
        {
            var impayes = await _impayeRepository.GetImpayesByPrestataireAsync(prestataireId);

            if (impayes == null || !impayes.Any())
            {
                return NotFound(new { message = "Aucun impayé trouvé pour ce prestataire." });
            }

            // Convertir les données en DTO
            var response = impayes.Select(i => new ImpayeWithClientDetailsDto
            {
                // Détails de l'impayé
                Idimpaye = i.Idimpaye,
                ref_impaye = i.ref_impaye,
                typeimpaye = i.typeimpaye,
                date_impaye = i.date_impaye,
                echeance_Principale = i.echeance_Principale,
                retard = i.retard,
                type_credit = i.type_credit,
                mt_total = i.mt_total,
                mt_impaye = i.mt_impaye,
                mt_encours = i.mt_encours,
                interet = i.interet,
                statut = i.statut,

                // Détails du client
                IdClient = i.Client?.IdClient ?? 0,
                refclient = i.Client?.refclient,
                Nom = i.Client?.Nom,
                Prenom = i.Client?.Prenom,
                email = i.Client?.email,
                telephone = i.Client?.telephone.ToString()
            }).ToList();

            return Ok(response);
        }
    }
}
