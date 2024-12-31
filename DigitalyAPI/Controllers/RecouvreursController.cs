using DigitalyAPI.Data;
using DigitalyAPI.Models.Domain;
using DigitalyAPI.Models.DTO;
using DigitalyAPI.Repositories.Implementation;
using DigitalyAPI.Repositories.Interface;
using DigitalyAPI.Services.IService;
using DigitalyAPI.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DigitalyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecouvreursController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRecouvreurRepository recouvreurRepository;
        private readonly ApplicationDBContext _dbContext;
        private readonly IRecouvreurService _recouvreurService;
    

        public RecouvreursController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IRecouvreurRepository recouvreurRepository, ApplicationDBContext dbContext, IRecouvreurService recouvreurService)
        {

            _userManager = userManager;
            _roleManager = roleManager;
            this.recouvreurRepository = recouvreurRepository;
            _dbContext = dbContext;
            _recouvreurService = recouvreurService;
          


        }


        [HttpPost("create-recouvreur")]
        public async Task<IActionResult> CreateRecouvreur(CreateRecouvreurDto request)
        {
            // Check if the email already exists
            var existingRecouvreur = await recouvreurRepository.GetByEmailAsync(request.email);
            if (existingRecouvreur != null)
            {
                // Return a conflict response if the email already exists
                return Conflict(new { message = "The email address is already in use." });
            }

            // Fetch the highest refrecouvreur to generate the next suffix
            var lastRecouvreur = await recouvreurRepository.GetLastRecouvreurAsync();
            int nextId = 1; // Default for the first record
            if (lastRecouvreur != null && !string.IsNullOrEmpty(lastRecouvreur.refrecouvreur))
            {
                string lastSuffix = lastRecouvreur.refrecouvreur.Replace("Rcv-", "");
                if (int.TryParse(lastSuffix, out int lastId))
                {
                    nextId = lastId + 1;
                }
            }

            // Generate the new refrecouvreur
            string newRefRecouvreur = $"Rcv-{nextId:D3}";

            // Map DTO to domain model
            var recouvreur = new Recouvreur
            {
                Profile = request.Profile,
                civilite = request.civilite,
                email = request.email,
                nom = request.nom,
                prenom = request.prenom,
                mobile = request.mobile,
                fax = request.fax,
                adresse = request.adresse,
                pays = request.pays,
                ville = request.ville,
                fonction = request.fonction,
                grade = request.grade,
                anciennete = request.anciennete,
                status = request.status,
                codepostal = request.codepostal,
                role = request.role,
                refrecouvreur = newRefRecouvreur // Set the generated reference
            };

            await recouvreurRepository.CreateAsync(recouvreur);

            // Domain model to DTO
            var response = new RecouvreurDto
            {
                IdRecouvreur = recouvreur.IdRecouvreur,
                Profile = recouvreur.Profile,
                civilite = recouvreur.civilite,
                email = recouvreur.email,
                nom = recouvreur.nom,
                prenom = recouvreur.prenom,
                mobile = recouvreur.mobile,
                fax = recouvreur.fax,
                adresse = recouvreur.adresse,
                pays = recouvreur.pays,
                ville = recouvreur.ville,
                fonction = recouvreur.fonction,
                grade = recouvreur.grade,
                anciennete = recouvreur.anciennete,
                status = recouvreur.status,
                codepostal = recouvreur.codepostal,
                role = recouvreur.role,
                refrecouvreur = recouvreur.refrecouvreur
            };

            return Ok(response);
        }





        // récupérer tous les recouvreurs


        [HttpGet("get-all-recouvreurs")]
        public async Task<IActionResult> GetAllRecouvreurs()
        {
            // Récupérer tous les recouvreurs avec leurs clients et impayés associés
            var recouvreurs = await _dbContext.recouvreurs
                .Include(r => r.Portefeuille)  // Inclure le portefeuille du recouvreur
                    .ThenInclude(p => p.Clients) // Inclure les clients associés au portefeuille
                        .ThenInclude(c => c.impayes) // Inclure les impayés des clients
                .ToListAsync();

            var recouvreursDto = recouvreurs.Select(recouvreur => new
            {
                IdRecouvreur = recouvreur.IdRecouvreur,
                Profile = recouvreur.Profile,
                civilite = recouvreur.civilite,
                email = recouvreur.email,
                nom = recouvreur.nom,
                prenom = recouvreur.prenom,
                mobile = recouvreur.mobile,
                fax = recouvreur.fax,
                adresse = recouvreur.adresse,
                pays = recouvreur.pays,
                ville = recouvreur.ville,
                fonction = recouvreur.fonction,
                grade = recouvreur.grade,
                anciennete = recouvreur.anciennete,
                status = recouvreur.status,
                codepostal = recouvreur.codepostal,
                role = recouvreur.role,
                refrecouvreur = recouvreur.refrecouvreur,
                PortefeuilleId = recouvreur.Portefeuille?.IdPortefeuille ?? 0, // Retourne 0 si IdPortefeuille est null
                NombreDossier = recouvreur.Portefeuille?.Clients
                    .Count(c => c.RecouvreurId == recouvreur.IdRecouvreur) ?? 0, // Compte uniquement les clients affectés à ce recouvreur

                // Calcul du TotalImpaye
                TotalImpaye = recouvreur.Portefeuille?.Clients
                    .Where(client => client.RecouvreurId == recouvreur.IdRecouvreur) // Filtrer les clients du recouvreur
                    .SelectMany(client => client.impayes) // Récupérer tous les impayés des clients
                    .Where(impaye => !string.IsNullOrEmpty(impaye.mt_impaye)) // Filtrer les impayés valides
                    .Sum(impaye =>
                    {
                        decimal montant = 0;
                        // Si mt_impaye contient des caractères autres que des chiffres, les supprimer avant la conversion
                        var montantSansTND = impaye.mt_impaye.Replace("TND", "").Trim();
                        if (decimal.TryParse(montantSansTND, out montant))
                        {
                            return montant; // Retourner le montant si la conversion est réussie
                        }
                        return 0; // Retourner 0 si la conversion échoue
                    }) + " TND" // Ajouter " TND" à la somme des impayés
            }).ToList();

            return Ok(recouvreursDto);
        }



        [HttpGet("get-recouvreur/{id}")]
        public async Task<IActionResult> GetRecouvreurById(int id)
        {
            var recouvreur = await recouvreurRepository.GetByIdAsync(id);

            if (recouvreur == null)
            {
                return NotFound();
            }

            var recouvreurDto = new RecouvreurDto
            {
                IdRecouvreur = recouvreur.IdRecouvreur,
                Profile = recouvreur.Profile,
                civilite = recouvreur.civilite,
                email = recouvreur.email,
                nom = recouvreur.nom,
                prenom = recouvreur.prenom,
                mobile = recouvreur.mobile,
                fax = recouvreur.fax,
                adresse = recouvreur.adresse,
                pays = recouvreur.pays,
                ville = recouvreur.ville,
                fonction = recouvreur.fonction,
                grade = recouvreur.grade,
                anciennete = recouvreur.anciennete,
                status = recouvreur.status,
                codepostal = recouvreur.codepostal,
                role = recouvreur.role,
                refrecouvreur = recouvreur.refrecouvreur



            };

            return Ok(recouvreurDto);
        }

        [HttpPut("update-recouvreur/{id}")]
        public async Task<IActionResult> UpdateRecouvreur(int id, UpdateRecouvreurRequestDTO request)
        {
            var recouvreur = new Recouvreur
            {
                Profile = request.Profile,
                civilite = request.civilite,
                email = request.email,
                nom = request.nom,
                prenom = request.prenom,
                mobile = request.mobile,
                fax = request.fax,
                adresse = request.adresse,
                pays = request.pays,
                ville = request.ville,
                fonction = request.fonction,
                grade = request.grade,
                anciennete = request.anciennete,
                status = request.status,
                codepostal = request.codepostal,
                role = request.role,
                refrecouvreur = request.refrecouvreur,


            };

            var updatedRecouvreur = await recouvreurRepository.UpdateAsync(id, recouvreur);

            if (updatedRecouvreur == null)
            {
                return NotFound();
            }

            var response = new RecouvreurDto
            {
                IdRecouvreur = updatedRecouvreur.IdRecouvreur,
                Profile = updatedRecouvreur.Profile,
                civilite = updatedRecouvreur.civilite,
                email = updatedRecouvreur.email,
                nom = updatedRecouvreur.nom,
                prenom = updatedRecouvreur.prenom,
                mobile = updatedRecouvreur.mobile,
                fax = updatedRecouvreur.fax,
                adresse = updatedRecouvreur.adresse,
                pays = updatedRecouvreur.pays,
                ville = updatedRecouvreur.ville,
                fonction = updatedRecouvreur.fonction,
                grade = updatedRecouvreur.grade,
                anciennete = updatedRecouvreur.anciennete,
                status = updatedRecouvreur.status,
                codepostal = updatedRecouvreur.codepostal,
                role = updatedRecouvreur.role,
                refrecouvreur = recouvreur.refrecouvreur,
            };

            return Ok(response);
        }

        [HttpDelete("delete-recouvreur/{id}")]
        public async Task<IActionResult> DeleteRecouvreur(int id)
        {
            var deletedRecouvreur = await recouvreurRepository.DeleteAsync(id);

            if (deletedRecouvreur == null)
            {
                return NotFound();
            }

            var response = new RecouvreurDto
            {
                IdRecouvreur = deletedRecouvreur.IdRecouvreur,
                Profile = deletedRecouvreur.Profile,
                civilite = deletedRecouvreur.civilite,
                email = deletedRecouvreur.email,
                nom = deletedRecouvreur.nom,
                prenom = deletedRecouvreur.prenom,
                mobile = deletedRecouvreur.mobile,
                fax = deletedRecouvreur.fax,
                adresse = deletedRecouvreur.adresse,
                pays = deletedRecouvreur.pays,
                ville = deletedRecouvreur.ville,
                fonction = deletedRecouvreur.fonction,
                grade = deletedRecouvreur.grade,
                anciennete = deletedRecouvreur.anciennete,
                status = deletedRecouvreur.status,
                codepostal = deletedRecouvreur.codepostal,
                role = deletedRecouvreur.role,
                refrecouvreur = deletedRecouvreur.refrecouvreur,


            };

            return Ok(response);
        }


        [HttpGet("get-recouvreurs-disponibles")]
        public async Task<IActionResult> GetRecouvreursDisponibles()
        {
            // Appel au repository pour récupérer les recouvreurs non affectés
            var recouvreursDisponibles = await recouvreurRepository.GetRecouvreursNonAffectesAsync();

            // Mapper en DTO
            var recouvreursDto = recouvreursDisponibles.Select(recouvreur => new RecouvreurDto
            {
                IdRecouvreur = recouvreur.IdRecouvreur,
                Profile = recouvreur.Profile,
                civilite = recouvreur.civilite,
                email = recouvreur.email,
                nom = recouvreur.nom,
                prenom = recouvreur.prenom,
                mobile = recouvreur.mobile,
                fax = recouvreur.fax,
                adresse = recouvreur.adresse,
                pays = recouvreur.pays,
                ville = recouvreur.ville,
                fonction = recouvreur.fonction,
                grade = recouvreur.grade,
                anciennete = recouvreur.anciennete,
                status = recouvreur.status,
                codepostal = recouvreur.codepostal,
                role = recouvreur.role,
                refrecouvreur = recouvreur.refrecouvreur,
                //  PortefeuilleId = recouvreur.PortefeuilleId
            }).ToList();

            return Ok(recouvreursDto);
        }

        [HttpGet("recouvreur-total-impaye/{recouvreurId}")]
        public async Task<IActionResult> GetTotalImpayeByRecouvreur(int recouvreurId)
        {
            // Récupérer le recouvreur avec les clients associés
            var recouvreurWithClients = await _dbContext.recouvreurs
                .Where(r => r.IdRecouvreur == recouvreurId)
                .Include(r => r.Clients)  // Inclure les clients associés
                .ThenInclude(c => c.impayes) // Inclure les impayés des clients
                .FirstOrDefaultAsync();

            if (recouvreurWithClients == null)
            {
                return NotFound("Recouvreur non trouvé");
            }

            // Calculer le montant total des impayés pour tous les clients associés à ce recouvreur
            var totalImpayeRecouvreur = recouvreurWithClients.Clients
                .SelectMany(client => client.impayes) // Récupérer tous les impayés des clients
                .Where(impaye => !string.IsNullOrEmpty(impaye.mt_impaye)) // Filtrer les impayés valides
                .Sum(impaye =>
                {
                    decimal montant = 0;
                    if (decimal.TryParse(impaye.mt_impaye.Replace("TND", "").Trim(), out montant))
                    {
                        return montant;
                    }
                    return 0; // Retourner 0 si la conversion échoue
                });

            // Retourner le montant total des impayés
            return Ok(new { RecouvreurId = recouvreurWithClients.IdRecouvreur, TotalImpaye = totalImpayeRecouvreur + " TND" });
        }

        [HttpGet("get-all-recouvreurs-total-impayes")]
        public async Task<IActionResult> GetAllRecouvreursTotalImpaye()
        {
            // Récupérer tous les recouvreurs avec leurs clients et impayés associés
            var recouvreursWithImpaye = await _dbContext.recouvreurs
                .Include(r => r.Clients)  // Inclure les clients associés
                    .ThenInclude(c => c.impayes) // Inclure les impayés des clients
                .ToListAsync();

            if (recouvreursWithImpaye == null || !recouvreursWithImpaye.Any())
            {
                return NotFound("Aucun recouvreur trouvé.");
            }

            // Calculer le total des impayés pour chaque recouvreur
            var recouvreursTotalImpaye = recouvreursWithImpaye.Select(recouvreur => new
            {
                RecouvreurId = recouvreur.IdRecouvreur,
                NomRecouvreur = $"{recouvreur.nom} {recouvreur.prenom}",
                TotalImpaye = recouvreur.Clients
                    .SelectMany(client => client.impayes) // Récupérer tous les impayés des clients
                    .Where(impaye => !string.IsNullOrEmpty(impaye.mt_impaye)) // Filtrer les impayés valides
                    .Sum(impaye =>
                    {
                        decimal montant = 0;
                        if (decimal.TryParse(impaye.mt_impaye.Replace("TND", "").Trim(), out montant))
                        {
                            return montant;
                        }
                        return 0; // Retourner 0 si la conversion échoue
                    }) + " TND"
            }).ToList();

            return Ok(recouvreursTotalImpaye);
        }

        [HttpGet("recouvreurs-impayes")]
        public async Task<IActionResult> GetRecouvreursWithClientImpayes()
        {
            var result = await _recouvreurService.GetRecouvreursWithClientImpayesAsync();
            return Ok(result);
        }

        [HttpGet("recouvreur-information/{id}")]
        public async Task<IActionResult> GetRecouvreurInformationById(int id)
        {
            var recouvreur = await _recouvreurService.GetRecouvreurInformationByIdAsync(id);

            if (recouvreur == null)
                return NotFound(new { message = "Recouvreur not found." });

            return Ok(recouvreur);
        }

        [HttpPost("create-recouvreurrrrr")]
        public async Task<IActionResult> CreateRecouvreurrr(CreateRecouvreurDto request)
        {
            // Step 1: Check if the email already exists
            var existingUser = await _userManager.FindByEmailAsync(request.email);
            if (existingUser != null)
            {
                return Conflict(new { message = "The email address is already in use." });
            }

            // Step 2: Check if the city is provided
            if (string.IsNullOrEmpty(request.ville))
            {
                return BadRequest("La ville est obligatoire.");
            }

            // Generate the next refrecouvreur
            var lastRecouvreur = await recouvreurRepository.GetLastRecouvreurAsync();
            int nextId = 1;
            if (lastRecouvreur?.refrecouvreur != null && lastRecouvreur.refrecouvreur.StartsWith("Rcv-"))
            {
                string lastSuffix = lastRecouvreur.refrecouvreur.Replace("Rcv-", "");
                if (int.TryParse(lastSuffix, out int lastId))
                {
                    nextId = lastId + 1;
                }
            }
            string newRefRecouvreur = $"Rcv-{nextId:D3}";

            // Generate a random password
            string generatedPassword = GenerateRandomPassword();

            // Create the Identity user
            var identityUser = new User
            {
                UserName = request.email,
                FirstName = request.nom,
                LastName = request.prenom,
                Email = request.email,
                EmailConfirmed = true,
                City = request.ville,
                Civilite = request.civilite,
                Country = request.pays,
                Dateofbirth = request.DateOfBirth,
                Fax = request.fax,
                Grade = request.grade,
                PhoneFix = request.Phonefix,
                PhoneNumber = request.mobile,
                PostalCode = request.codepostal,
                longInPosition = request.anciennete,
            };

            var identityResult = await _userManager.CreateAsync(identityUser, generatedPassword);
            if (!identityResult.Succeeded)
            {
                return StatusCode(500, new
                {
                    message = "Failed to create user in AspNetUsers.",
                    errors = identityResult.Errors
                });
            }

            // Assign a role to the user
            string roleName = request.role.ToString();
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                {
                    return StatusCode(500, new
                    {
                        message = "Failed to create role.",
                        errors = roleResult.Errors
                    });
                }
            }
            await _userManager.AddToRoleAsync(identityUser, roleName);

            // Create the Recouvreur object and set the UserId
            var recouvreur = new Recouvreur
            {
                Profile = request.Profile,
                civilite = request.civilite,
                email = request.email,
                nom = request.nom,
                prenom = request.prenom,
                mobile = request.mobile,
                fax = request.fax,
                adresse = request.adresse,
                pays = request.pays,
                ville = request.ville,
                fonction = request.fonction,
                grade = request.grade,
                anciennete = request.anciennete,
                status = request.status,
                codepostal = request.codepostal,
                role = request.role,
                refrecouvreur = newRefRecouvreur,
                UserId = identityUser.Id // Set the UserId here
            };

            // Save the Recouvreur
            await recouvreurRepository.CreateAsync(recouvreur);

            // Prepare the response
            var response = new RecouvreurDto
            {
                IdRecouvreur = recouvreur.IdRecouvreur,
                Profile = recouvreur.Profile,
                civilite = recouvreur.civilite,
                email = recouvreur.email,
                nom = recouvreur.nom,
                prenom = recouvreur.prenom,
                mobile = recouvreur.mobile,
                fax = recouvreur.fax,
                adresse = recouvreur.adresse,
                pays = recouvreur.pays,
                ville = recouvreur.ville,
                fonction = recouvreur.fonction,
                grade = recouvreur.grade,
                anciennete = recouvreur.anciennete,
                status = recouvreur.status,
                codepostal = recouvreur.codepostal,
                role = recouvreur.role,
                refrecouvreur = recouvreur.refrecouvreur
            };

            return Ok(new
            {
                recouvreur.IdRecouvreur,
                recouvreur.email,
                recouvreur.refrecouvreur,
                role = roleName,
                generatedPassword, // Include for testing, remove in production
                message = "Recouvreur created successfully, user added to AspNetUsers."
            });
        }


        // Helper method to generate a random password
        private string GenerateRandomPassword(int length = 12)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789@#$%&*!";
            var chars = new char[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteBuffer = new byte[length];
                rng.GetBytes(byteBuffer);
                for (int i = 0; i < length; i++)
                {
                    chars[i] = validChars[byteBuffer[i] % validChars.Length];
                }
            }
            return new string(chars);
        }

        [HttpGet("get-recouvreur-by-userid/{userId}")]
        public async Task<IActionResult> GetRecouvreurByUserId(string userId)
        {
            var recouvreur = await recouvreurRepository.GetByUserIdAsync(userId);
            if (recouvreur == null)
            {
                return NotFound(new { message = "Recouvreur not found for the given UserId." });
            }

            return Ok(recouvreur);
        }
        // Endpoint pour récupérer les impayés d'un recouvreur
        [HttpGet("{recouvreurId}/impayes")]
        public async Task<IActionResult> GetImpayesByRecouvreur(int recouvreurId)
        {
            var impayes = await recouvreurRepository.GetImpayesByRecouvreurAsync(recouvreurId);

            if (impayes == null || !impayes.Any())
            {
                return NotFound(new { message = "Aucun impayé trouvé pour ce recouvreur." });
            }

            return Ok(impayes);
        }


        /*

       [HttpPost("create-recouvreurrrr")]
    public async Task<IActionResult> CreateRecouvreurrrr(RecouvreurDto request)
    {
        // Check if the email already exists
        var existingRecouvreur = await recouvreurRepository.GetByEmailAsync(request.email);
        if (existingRecouvreur != null)
        {
            return Conflict(new { message = "The email address is already in use." });
        }

        // Fetch the last recouvreur ref to generate a new suffix
        var lastRecouvreur = await recouvreurRepository.GetLastRecouvreurAsync();
        int nextId = 1;
        if (lastRecouvreur != null && !string.IsNullOrEmpty(lastRecouvreur.refrecouvreur))
        {
            string lastSuffix = lastRecouvreur.refrecouvreur.Replace("Rcv-", "");
            if (int.TryParse(lastSuffix, out int lastId))
            {
                nextId = lastId + 1;
            }
        }

        // Generate the new refrecouvreur
        string newRefRecouvreur = $"Rcv-{nextId:D3}";

        // Map DTO to domain model
        var recouvreur = new Recouvreur
        {
            Profile = request.Profile,
            civilite = request.civilite,
            email = request.email,
            nom = request.nom,
            prenom = request.prenom,
            mobile = request.mobile,
            fax = request.fax,
            adresse = request.adresse,
            pays = request.pays,
            ville = request.ville,
            fonction = request.fonction,
            grade = request.grade,
            anciennete = request.anciennete,
            status = request.status,
            codepostal = request.codepostal,
            role = request.role,
            refrecouvreur = newRefRecouvreur
        };

        // Save the Recouvreur in the database
        await recouvreurRepository.CreateAsync(recouvreur);

        // ----- Step 2: Generate a Random Password -----
        string generatedPassword = GenerateRandomPassword();

        // ----- Step 3: Add the Recouvreur to AspNetUsers -----
        var identityUser = new IdentityUser
        {
            UserName = request.email,
            Email = request.email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(identityUser, generatedPassword);
        if (!result.Succeeded)
        {
            return StatusCode(500, new { message = "Failed to create user in AspNetUsers." });
        }

            // ----- Step 4: Assign Role -----
            // Directly use the enum value from the request
            rolerecouv role = request.role;

            // Convert the enum to its string representation for IdentityRole
            string roleName = role.ToString();

            // Check if the role exists in the Identity system
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Assign the role to the user
            await _userManager.AddToRoleAsync(identityUser, roleName);





            // ----- Step 5: Return Response -----
            return Ok(new
        {
            recouvreur.IdRecouvreur,
            recouvreur.email,
            recouvreur.refrecouvreur,
            role,
            generatedPassword, // Return the generated password for testing or send via email
            message = "Recouvreur created successfully, user added to AspNetUsers."
        });
    }




    // Helper Method to Generate Random Password
    private string GenerateRandomPassword()
    {
        const int length = 10;
        const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789@#$%&*!";

        using (var rng = new RNGCryptoServiceProvider())
        {
            var chars = new char[length];
            var byteBuffer = new byte[length];

            rng.GetBytes(byteBuffer);

            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[byteBuffer[i] % validChars.Length];
            }

            return new string(chars);
        }
    }
*/



    }



}

