using DigitalyAPI.Models.Domain;
using DigitalyAPI.Models.DTO;
using DigitalyAPI.Repositories.Implementation;
using DigitalyAPI.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestatairesController : ControllerBase
    {

        private readonly IPrestataireRepository prestataireRepository;

        public PrestatairesController(IPrestataireRepository prestataireRepository)
        {
            this.prestataireRepository = prestataireRepository;

        }

        [HttpPost("create-prestataire")]
        public async Task<IActionResult> CreatePrestataire(PrestataireDto request)
        {
            // Check if the email already exists
            var existingPrestataire = await prestataireRepository.GetByEmailAsync(request.email);
            if (existingPrestataire != null)
            {
                // Return a conflict response if the email already exists
                return Conflict(new { message = "The email address is already in use." });
            }

            // Fetch the highest refprestataire to generate the next suffix
            var lastPrestataire = await prestataireRepository.GetLastPrestataireAsync();
            int nextId = 1; // Default for the first record
            if (lastPrestataire != null && !string.IsNullOrEmpty(lastPrestataire.refprestataire))
            {
                // Extract the numeric part of the last refprestataire
                string lastSuffix = lastPrestataire.refprestataire.Replace("Prest-", "");
                if (int.TryParse(lastSuffix, out int lastId))
                {
                    nextId = lastId + 1;
                }
            }

            // Generate the new refprestataire
            string newRefPrestataire = $"Prest-{nextId:D3}";

            // Map DTO to domain model
            var newPrestataire = new Prestataire
            {
                Profile = request.Profile,
                civilite = request.civilite,
                email = request.email,
                nom = request.nom,
                prenom = request.prenom,
                mobile = request.mobile,
                fax = request.fax,
                adresse = request.adresse,
                agence = request.agence,
                pays = request.pays,
                ville = request.ville,
                status = request.status,
                codepostal = request.codepostal,
                refprestataire = newRefPrestataire, // Set the generated reference
                zone_geo = request.zone_geo,
                nombre_dossiers_encharge = request.nombre_dossiers_encharge,
                role = request.role,
                anciennete = request.anciennete,
            };

            // Save to the database
            await prestataireRepository.CreateAsync(newPrestataire);

            // Map domain model to DTO
            var response = new PrestataireDto
            {
                IdPrestataire = newPrestataire.IdPrestataire,
                Profile = newPrestataire.Profile,
                civilite = newPrestataire.civilite,
                email = newPrestataire.email,
                nom = newPrestataire.nom,
                prenom = newPrestataire.prenom,
                mobile = newPrestataire.mobile,
                fax = newPrestataire.fax,
                adresse = newPrestataire.adresse,
                agence = newPrestataire.agence,
                pays = newPrestataire.pays,
                ville = newPrestataire.ville,
                status = newPrestataire.status,
                codepostal = newPrestataire.codepostal,
                refprestataire = newPrestataire.refprestataire,
                zone_geo = newPrestataire.zone_geo,
                nombre_dossiers_encharge = newPrestataire.nombre_dossiers_encharge,
                role = newPrestataire.role,
                anciennete = newPrestataire.anciennete,
            };

            return Ok(response);
        }

        // Récupérer tous les prestataires
        // [Authorize]
        [HttpGet("get-all-prestataires")]
        public async Task<IActionResult> GetAllPrestataires()
        {
            var prestataires = await prestataireRepository.GetAllWithImpayesAsync();

            // Mapper les prestataires de domaine en DTOs
            var prestatairesDto = prestataires.Select(prestataire => new PrestataireDto
            {
                IdPrestataire = prestataire.IdPrestataire,
                Profile = prestataire.Profile,
                civilite = prestataire.civilite,
                email = prestataire.email,
                nom = prestataire.nom,
                prenom = prestataire.prenom,
                mobile = prestataire.mobile,
                fax = prestataire.fax,
                adresse = prestataire.adresse,
                agence = prestataire.agence,
                pays = prestataire.pays,
                ville = prestataire.ville,
                status = prestataire.status,
                codepostal = prestataire.codepostal,
                refprestataire = prestataire.refprestataire,
                zone_geo = prestataire.zone_geo,
                role = prestataire.role,
                anciennete = prestataire.anciennete,
                // Nombre de dossiers en charge = nombre d'impayés associés
                nombre_dossiers_encharge = prestataire.Impayes.Count
            }).ToList();

            return Ok(prestatairesDto);
        }


        [HttpGet("get-prestataire/{id}")]
        public async Task<IActionResult> GetPrestataireById(int id)
        {
            // Récupérer le prestataire avec ses impayés associés
            var prestataire = await prestataireRepository.GetByIdAsync(id);

            if (prestataire == null)
            {
                return NotFound();
            }

            // Calculer le montant total des impayés associés
            decimal totalMontantImpayes = prestataire.Impayes?
     .Sum(i =>
     {
         // Nettoyer la valeur mt_impaye avant de la convertir
         string montantNettoye = i.mt_impaye?.Replace("TND", "").Trim();
         return decimal.TryParse(montantNettoye, out var montant) ? montant : 0;
     }) ?? 0;
            // Mapper le modèle Prestataire au DTO
            var prestataireDto = new PrestataireDto
            {
                IdPrestataire = prestataire.IdPrestataire,
                Profile = prestataire.Profile,
                civilite = prestataire.civilite,
                email = prestataire.email,
                nom = prestataire.nom,
                prenom = prestataire.prenom,
                mobile = prestataire.mobile,
                fax = prestataire.fax,
                adresse = prestataire.adresse,
                agence = prestataire.agence,
                pays = prestataire.pays,
                ville = prestataire.ville,
                status = prestataire.status,
                codepostal = prestataire.codepostal,
                refprestataire = prestataire.refprestataire,
                zone_geo = prestataire.zone_geo,
                anciennete = prestataire.anciennete,
                role = prestataire.role,

                // Ajouter le nombre d'impayés et concaténer avec le montant total
                nombre_dossiers_encharge = prestataire.Impayes?.Count ?? 0,
                retard_paiement = $"{totalMontantImpayes:F2} TND" // Ajouter "TND" comme unité
            };

            return Ok(prestataireDto);
        }




        [HttpDelete("delete-prestataire/{id}")]
        public async Task<IActionResult> DeletePrestataire(int id)
        {
            var deletedprestataire = await prestataireRepository.DeleteAsync(id);

            if (deletedprestataire == null)
            {
                return NotFound("prestataire unfound");
            }

            return Ok("deleted with sucess");
        }

       /* [HttpPut("archive-prestataire/{id}")]
        public async Task<IActionResult> ArchivePrestataire(int id)
        {
            var archivedPrestataire = await prestataireRepository.ArchiveAsync(id);

            if (archivedPrestataire == null)
            {
                return NotFound("Prestataire non trouvé");
            }

            return Ok("Prestataire archivé avec succès");
        }*/


        // Mise à jour d'un prestataire par identifiant
        [HttpPut("update-prestataire/{id}")]
        public async Task<IActionResult> UpdatePrestataire(int id, PrestataireDto request)
        {
            var prestataire = new Prestataire
            {
                nom = request.nom,
                prenom = request.prenom,
                Profile = request.Profile,
                civilite = request.civilite,
                email = request.email,
                mobile = request.mobile,
                fax = request.fax,
                adresse = request.adresse,
                agence = request.agence,
                pays = request.pays,
                ville = request.ville,
                status = request.status,
                codepostal = request.codepostal,
                refprestataire = request.refprestataire,
                zone_geo = request.zone_geo,
                nombre_dossiers_encharge = request.nombre_dossiers_encharge,
                role = request.role,
                anciennete = request.anciennete,
            };

            var updatedPrestataire = await prestataireRepository.UpdateAsync(id, prestataire);

            if (updatedPrestataire == null)
            {
                return NotFound();
            }

            var response = new PrestataireDto
            {
                IdPrestataire = updatedPrestataire.IdPrestataire,
                nom = updatedPrestataire.nom,
                prenom = updatedPrestataire.prenom,
                Profile = updatedPrestataire.Profile,
                civilite = updatedPrestataire.civilite,
                email = updatedPrestataire.email,
                mobile = updatedPrestataire.mobile,
                fax = updatedPrestataire.fax,
                adresse = updatedPrestataire.adresse,
                agence = updatedPrestataire.agence,
                pays = updatedPrestataire.pays,
                ville = updatedPrestataire.ville,
                status = updatedPrestataire.status,
                codepostal = updatedPrestataire.codepostal,
                refprestataire = updatedPrestataire.refprestataire,
                zone_geo = updatedPrestataire.zone_geo,
                nombre_dossiers_encharge = updatedPrestataire.nombre_dossiers_encharge,
                role = updatedPrestataire.role,
                anciennete=updatedPrestataire.anciennete,
            };

            return Ok(response);
        }

        [HttpGet("get-impayes-by-prestataire/{id}")]
        public async Task<IActionResult> GetImpayesByPrestataireId(int id)
        {
            // Récupérer les impayés associés au prestataire
            var impayes = await prestataireRepository.GetImpayesByPrestataireIdAsync(id);

            if (impayes == null || !impayes.Any())
            {
                return NotFound(new { message = "No unpaid items found for this prestataire." });
            }

            // Mapper les impayés en DTO
            var impayesDto = impayes.Select(impaye => new ImpayeDto
            {
                Idimpaye = impaye.Idimpaye,
                ref_impaye = impaye.ref_impaye,
                typeimpaye = impaye.typeimpaye,
                date_impaye = impaye.date_impaye,
                echeance_Principale = impaye.echeance_Principale,
                retard = impaye.retard,
                type_credit = impaye.type_credit,
                mt_total = impaye.mt_total,
                mt_impaye = impaye.mt_impaye,
                mt_encours = impaye.mt_encours,
                interet = impaye.interet,
                statut = impaye.statut,
                PrestataireId = impaye.PrestataireId
            }).ToList();

            return Ok(impayesDto);
        }


    }
}
