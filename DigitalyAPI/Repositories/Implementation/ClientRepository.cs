using DigitalyAPI.Data;
using DigitalyAPI.Models.Domain;
using DigitalyAPI.Models.DTO;
using DigitalyAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DigitalyAPI.Repositories.Implementation
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDBContext dBContext;

        public ClientRepository(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<Client> CreateAsync(Client client)
        {
            await dBContext.clients.AddAsync(client);
            await dBContext.SaveChangesAsync();

            return client;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var client = await dBContext.clients.FirstOrDefaultAsync(c => c.IdClient == id);
            if (client == null) return false;

            dBContext.clients.Remove(client);
            await dBContext.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await dBContext.clients.Include(c => c.compteBancaire).ToListAsync();
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            return await dBContext.clients.Include(c => c.compteBancaire).FirstOrDefaultAsync(c => c.IdClient == id);
        }


        public async Task<Client?> UpdateAsync(int id, Client client)
        {
            var existingClient = await dBContext.clients.Include(c => c.compteBancaire).FirstOrDefaultAsync(c => c.IdClient == id);
            if (existingClient == null) return null;

            // Mise à jour des propriétés
            existingClient.Nom = client.Nom;
            existingClient.Prenom = client.Prenom;
            existingClient.profession = client.profession;
            existingClient.telephone = client.telephone;
            existingClient.refclient = client.refclient;
            existingClient.Nationnalite = client.Nationnalite;
            existingClient.adresse = client.adresse;
            existingClient.email = client.email;
            existingClient.situationfamiliale = client.situationfamiliale;
            existingClient.Datedenaiss = client.Datedenaiss;
            existingClient.Fonction = client.Fonction;
            existingClient.Anciennete = client.Anciennete;

            await dBContext.SaveChangesAsync();
            return existingClient;
        }


        public async Task<Client> GetClientWithImpayesAsync(int clientId)
        {
            return await dBContext.clients
                .Include(c => c.impayes)  // Charger les impayés associés
                .FirstOrDefaultAsync(c => c.IdClient == clientId);
        }
        public async Task<IEnumerable<Client>> GetAllClientsWithImpayesAsync()
        {
            return await dBContext.clients
                .Include(c => c.impayes) // Inclure les impayés liés à chaque client
                    //    .Where(c => c.PortefeuilleId != null) // Optionnel : Si vous voulez filtrer ceux qui n'ont pas de portefeuille

                .ToListAsync();
        }

        public async Task<List<Client>> GetClientsWithBankAccountsAsync()
        {
            return await dBContext.clients
                .Include(c => c.compteBancaire) // Inclure le compte bancaire
                .Select(c => new Client
                {
                    IdClient = c.IdClient,
                    refclient = c.refclient,
                    Nom = c.Nom,
                    Prenom = c.Prenom,
                    Nationnalite = c.Nationnalite,
                    profession = c.profession,
                    adresse = c.adresse,
                    email = c.email,
                    situationfamiliale = c.situationfamiliale,
                    Datedenaiss = c.Datedenaiss,
                    telephone = c.telephone,
                    Fonction = c.Fonction,
                    Anciennete = c.Anciennete,
                    compteBancaire = c.compteBancaire // Inclure uniquement les comptes bancaires
                                                      // Exclure les impayés en ne les ajoutant pas ici
                })
                .ToListAsync();
        }

        public async Task<Client?> GetClientWithAccountByIdAsync(int id)
        {
            return await dBContext.Set<Client>()
                .Include(c => c.compteBancaire)
                .FirstOrDefaultAsync(c => c.IdClient == id);
        }

        public async Task<bool> AssignClientToPortefeuilleAsync(int clientId, int portefeuilleId)
        {
            var client = await dBContext.clients.FindAsync(clientId);
            var portefeuille = await dBContext.portefeuilles.FindAsync(portefeuilleId);

            if (client == null || portefeuille == null)
                return false;

            // Affecter le client au portefeuille
            client.PortefeuilleId = portefeuilleId;
            client.Portefeuille = portefeuille;

            // Sauvegarder les modifications
            await dBContext.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Client>> GetClientsWithImpayesByPortefeuilleAsync(int portefeuilleId)
        {
            return await dBContext.clients
                .Where(c => c.PortefeuilleId == portefeuilleId) // Filtrer par PortefeuilleId
                .Include(c => c.impayes) // Inclure les impayés
                .ToListAsync();
        }
        public async Task<bool> AssignClientsToRecouvreurAsync(List<int> clientIds, int recouvreurId)
        {
            // Vérifier si le recouvreur existe
            var recouvreur = await dBContext.recouvreurs
                .FirstOrDefaultAsync(r => r.IdRecouvreur == recouvreurId);

            if (recouvreur == null)
            {
                return false; // Recouvreur introuvable
            }

            // Récupérer les clients par leurs identifiants
            var clients = await dBContext.clients
                .Where(c => clientIds.Contains(c.IdClient))
                .ToListAsync();

            if (clients == null || clients.Count != clientIds.Count)
            {
                return false; // Certains clients n'ont pas été trouvés
            }

            // Affecter le recouvreur à chaque client
            foreach (var client in clients)
            {
                client.RecouvreurId = recouvreurId;
            }

            // Sauvegarder les modifications
            await dBContext.SaveChangesAsync();

            return true;
        }



    }


}
