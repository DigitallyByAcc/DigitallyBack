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
    //https://localhost:xxxx/api/clients
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository clientRepository;
        private readonly ApplicationDBContext dbcontext;

        public ClientsController( IClientRepository clientRepository,ApplicationDBContext dBContext)
        {
            this.clientRepository = clientRepository;
            this.dbcontext=dBContext;
        }
       
    

        //ajout d'un client 
       // POST: api/Clients/create-client
        [HttpPost("create-client")]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var client = new Client
            {
                Nom = request.Nom,
                Prenom = request.Prenom,
                profession = request.profession,
                Nationnalite = request.Nationnalite,
                telephone = request.telephone,
                refclient = request.refclient,
                adresse = request.adresse,
                email = request.email,
                situationfamiliale = request.situationfamiliale,
                Datedenaiss = request.Datedenaiss,
                Fonction = request.Fonction,
                Anciennete = request.Anciennete,
            };

            var createdClient = await clientRepository.CreateAsync(client);
            var response = new ClientDto
            {
                IdClient = createdClient.IdClient,
                Nom = createdClient.Nom,
                Prenom = createdClient.Prenom,
                profession = createdClient.profession,
                Nationnalite = createdClient.Nationnalite,
                telephone = createdClient.telephone,
                refclient = createdClient.refclient,
                adresse = createdClient.adresse,
                email = createdClient.email,
                situationfamiliale = createdClient.situationfamiliale,
                Datedenaiss = createdClient.Datedenaiss,
                Fonction = createdClient.Fonction,
                Anciennete = createdClient.Anciennete
            };

            return CreatedAtAction(nameof(GetClientById), new { id = createdClient.IdClient }, response);
        }

        [HttpGet("get-clientwithimpayes/{id}")]
        public async Task<IActionResult> GetClientWithImpayes(int id)
        {
            var client = await clientRepository.GetClientWithImpayesAsync(id);

            if (client == null)
            {
                return NotFound("Client non trouvé.");
            }

            return Ok(client);
        }

        [HttpGet("get-client-withimpayes")]
        public async Task<IActionResult> GetAllClientsWithImpayes()
        {
            var clients = await clientRepository.GetAllClientsWithImpayesAsync();

            if (clients == null || !clients.Any())
            {
                return NotFound("Aucun client trouvé avec leurs impayés.");
            }

            return Ok(clients);
        }

        // récupérer tous les clients
        [HttpGet("get-all-clients")]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await clientRepository.GetAllAsync();
            var clientsDto = clients.Select(client => new ClientDto
            {
                IdClient = client.IdClient,
                Nom = client.Nom,
                Prenom = client.Prenom,
                profession = client.profession,
                Nationnalite = client.Nationnalite,
                telephone = client.telephone,
                refclient = client.refclient,
                adresse = client.adresse,
                email = client.email,
                situationfamiliale = client.situationfamiliale,
                Datedenaiss = client.Datedenaiss,
                Fonction = client.Fonction,
                Anciennete = client.Anciennete
            }).ToList();

            return Ok(clientsDto);
        }

        //  récupérer un client par son identifiant
        [HttpGet("get-client/{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var client = await clientRepository.GetByIdAsync(id);
            if (client == null) return NotFound("Client not found");

            var clientDto = new ClientDto
            {
                IdClient = client.IdClient,
                Nom = client.Nom,
                Prenom = client.Prenom,
                profession = client.profession,
                Nationnalite = client.Nationnalite,
                telephone = client.telephone,
                refclient = client.refclient,
                adresse = client.adresse,
                email = client.email,
                situationfamiliale = client.situationfamiliale,
                Datedenaiss = client.Datedenaiss,
                Fonction = client.Fonction,
                Anciennete = client.Anciennete
            };

            return Ok(clientDto);
        }

        // PUT: api/Clients/update-client/{id}
        [HttpPut("update-client/{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] UpdateClientRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var client = new Client
            {
                Nom = request.Nom,
                Prenom = request.Prenom,
                profession = request.profession,
                Nationnalite = request.Nationnalite,
                telephone = request.telephone,
                refclient = request.refclient,
                adresse = request.adresse,
                email = request.email,
                situationfamiliale = request.situationfamiliale,
                Datedenaiss = request.Datedenaiss,
                Fonction = request.Fonction,
                Anciennete = request.Anciennete
            };

            var updatedClient = await clientRepository.UpdateAsync(id, client);
            if (updatedClient == null) return NotFound("Client not found");

            var response = new ClientDto
            {
                IdClient = updatedClient.IdClient,
                Nom = updatedClient.Nom,
                Prenom = updatedClient.Prenom,
                profession = updatedClient.profession,
                Nationnalite = updatedClient.Nationnalite,
                telephone = updatedClient.telephone,
                refclient = updatedClient.refclient,
                adresse = updatedClient.adresse,
                email = updatedClient.email,
                situationfamiliale = updatedClient.situationfamiliale,
                Datedenaiss = updatedClient.Datedenaiss,
                Fonction = updatedClient.Fonction,
                Anciennete = updatedClient.Anciennete
            };

            return Ok(response);
        }
        [HttpDelete("delete-client/{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var deleted = await clientRepository.DeleteAsync(id);
            if (!deleted) return NotFound("Client not found");

            return NoContent();
        }

        [HttpGet("get-all-clients-with-accounts")]
        public async Task<IActionResult> GetAllClientsWithAccounts()
        {
            try
            {
                var clients = await clientRepository.GetClientsWithBankAccountsAsync();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                // Gérer les erreurs et retourner une réponse appropriée
                return StatusCode(500, new { message = "Erreur interne", details = ex.Message });
            }
        }


        [HttpGet("get-clientwithaccountbank/{id}")]
        public async Task<IActionResult> GetClientAccoutnById(int id)
        {
            var client = await clientRepository.GetClientWithAccountByIdAsync(id);

            if (client == null)
            {
                return NotFound(new { Message = "Client not found." });
            }

            return Ok(client);
        }

        [HttpPost("assign-to-portefeuille")]
        public async Task<IActionResult> AssignClientToPortefeuille([FromBody] AssignClientToPortefeuilleDto model)
        {
            // Vérifier que les données sont valides
            if (model.ClientId <= 0 || model.PortefeuilleId <= 0)
                return BadRequest("ClientId et PortefeuilleId sont nécessaires.");

            var result = await clientRepository.AssignClientToPortefeuilleAsync(model.ClientId, model.PortefeuilleId);

            if (result)
                return Ok("Client affecté au portefeuille avec succès.");

            return NotFound("Client ou portefeuille non trouvé.");
        }
        [HttpGet("impayes/portefeuille/{portefeuilleId}")]
        public async Task<IActionResult> GetClientsWithImpayesByPortefeuille(int portefeuilleId)
        {
            var clients = await clientRepository.GetClientsWithImpayesByPortefeuilleAsync(portefeuilleId);

            if (clients == null || !clients.Any())
            {
                return NotFound("Aucun impayé trouvé pour ce portefeuille.");
            }

            return Ok(clients);
        }


        [HttpPost("assign-clients-to-recouvreur")]
        public async Task<IActionResult> AssignClientsToRecouvreur([FromBody] AssignClientsToRecouvreurDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Vérifier si la liste des clients n'est pas vide
            if (request.ClientIds == null || !request.ClientIds.Any())
            {
                return BadRequest("La liste des identifiants des clients ne peut pas être vide.");
            }

            // Appeler le repository pour effectuer l'affectation
            var result = await clientRepository.AssignClientsToRecouvreurAsync(request.ClientIds, request.RecouvreurId);

            if (result)
            {
                return Ok("Clients affectés au recouvreur avec succès.");
            }

            return NotFound("Recouvreur non trouvé ou certains clients introuvables.");
        }

        [HttpGet("clients-with-total-impaye")]
        public async Task<IActionResult> GetClientsWithTotalImpaye()
        {
            // Récupérer les clients et leurs impayés sans effectuer la conversion directement dans la requête LINQ
            var clientsWithImpaye = await dbcontext.clients
                .Select(client => new
                {
                    client.IdClient,
                    client.Nom,
                    client.Prenom,
                    Impayes = client.impayes
                        .Where(impaye => !string.IsNullOrEmpty(impaye.mt_impaye)) // Filtre les impayés valides
                        .ToList() // Charger les impayés en mémoire pour effectuer la conversion en C#
                })
                .ToListAsync(); // Charger les données en mémoire

            // Ensuite, effectuer la somme et la conversion en mémoire
            var clientsWithTotalImpaye = clientsWithImpaye.Select(client => new
            {
                client.IdClient,
                client.Nom,
                client.Prenom,
                // Calculer la somme des impayés et ajouter "TND" à la somme totale
                TotalImpaye = client.Impayes
                    .Sum(impaye =>
                    {
                        decimal montant = 0;
                        // Convertir après récupération des données en mémoire
                        if (decimal.TryParse(impaye.mt_impaye.Replace("TND", "").Trim(), out montant))
                        {
                            return montant;
                        }
                        return 0; // Si la conversion échoue, retourne 0
                    })
            })
            .Select(client => new
            {
                client.IdClient,
                client.Nom,
                client.Prenom,
                // Concaténer "TND" au montant total
                TotalImpaye = client.TotalImpaye.ToString("0.##") + " TND" // Formater le montant avec 2 décimales et ajouter "TND"
            })
            .ToList();

            return Ok(clientsWithTotalImpaye);
        }
         










    }
}
