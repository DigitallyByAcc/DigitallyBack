using DigitalyAPI.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace DigitalyAPI.Data
{
   // public class ApplicationDBContext : DbContext
    public class ApplicationDBContext:IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions  options ) : base(options)
        { 
        
        
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            seedRoles(modelBuilder);
          
            // Refresh token 
            modelBuilder.Entity<User>()
        .Property(u => u.RefreshToken)
        .IsRequired() // Ensures the column is NOT NULL
        .HasDefaultValue(string.Empty); // Sets a default value

            // Configurer la relation entre Impayé et Portefeuille
            /*  modelBuilder.Entity<Impaye>()
                  .HasOne(i => i.Portefeuille)
                  .WithMany(p => p.Impayes)
                  .HasForeignKey(i => i.PortefeuilleFK)
                  .OnDelete(DeleteBehavior.Cascade);*/

            // Relation one-to-many entre Portefeuille et Recouvreur
            modelBuilder.Entity<Recouvreur>()
                .HasOne(p => p.Portefeuille)
                .WithMany(r => r.Recouvreurs)
                .HasForeignKey(p => p.PortefeuilleId)
                .OnDelete(DeleteBehavior.SetNull)// Ou DeleteBehavior.Cascade si vous souhaitez supprimer aussi les recouvreurs
                .IsRequired(false); // Rendre la clé étrangère optionnelle

            // Relation one to one ente client et compteBancaire
            modelBuilder.Entity<CompteBancaire>()
              .HasOne(c => c.Client)
              .WithOne(c => c.compteBancaire)
              .HasForeignKey<CompteBancaire>(c => c.ClientId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Cascade); // Si un Client est supprimé, son CompteBancaire le sera aussi

            // Relation one-to-many entre Client et impayes
            modelBuilder.Entity<Impaye>()
                .HasOne(c => c.Client)
                .WithMany(i => i.impayes)
                .HasForeignKey(c => c.ClientId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            // Relation one-to-many entre Prestataire et impayes

            modelBuilder.Entity<Impaye>()
                        .HasOne(i => i.Prestataire) // Chaque impayé a un prestataire
                        .WithMany(p => p.Impayes) // Un prestataire peut avoir plusieurs impayés
                        .HasForeignKey(i => i.PrestataireId) // Clé étrangère
                        .OnDelete(DeleteBehavior.Restrict); // Empêche la suppression cascade

            // Relation un-à-plusieurs entre Portefeuille et Clients
            modelBuilder.Entity<Client>()
       .HasOne(c => c.Portefeuille)
       .WithMany(p => p.Clients)
       .HasForeignKey(c => c.PortefeuilleId)
       .OnDelete(DeleteBehavior.Cascade);

            // Configuration de la relation entre Recouvreur et Client (un recouvreur peut avoir plusieurs clients, chaque client est lié à un seul recouvreur)
            modelBuilder.Entity<Client>()
      .HasOne(c => c.Recouvreur) // Un client a un recouvreur
      .WithMany(r => r.Clients)  // Un recouvreur a plusieurs clients
      .HasForeignKey(c => c.RecouvreurId)
      .OnDelete(DeleteBehavior.SetNull); // Si le recouvreur est supprimé, la référence est mise à null

             // Conversion de l'énumération rolerecouv en chaîne de caractères
    modelBuilder.Entity<Recouvreur>()
        .Property(r => r.role)
        .HasConversion(
            v => v.ToString(),
            v => (rolerecouv)Enum.Parse(typeof(rolerecouv), v)
        );
            modelBuilder.Entity<Recouvreur>()
                         .HasOne(r => r.user)
                         .WithMany() // Assuming one user can have multiple roles, otherwise use `WithOne()`
                          .HasForeignKey(r => r.UserId)
                           .OnDelete(DeleteBehavior.Restrict);
        }

        private static void seedRoles (ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "RecouvreurAimable", ConcurrencyStamp = "2", NormalizedName = "RecouvreurAimable" },
                new IdentityRole() { Name = "RecouvreurContentieux", ConcurrencyStamp = "3", NormalizedName = "RecouvreurContentieux" }



                );

        }

        public DbSet<Client> clients { get; set; }

        public DbSet<Recouvreur> recouvreurs { get; set; }

        public DbSet<Portefeuille> portefeuilles { get; set; }
        public DbSet<Prestataire> prestataires { get; set; }

        public DbSet<CompteBancaire> comptesBancaires { get;set; }

        public DbSet<Impaye> impayes { get; set; }

        //public DbSet<User> users { get; set; }

    }

}
