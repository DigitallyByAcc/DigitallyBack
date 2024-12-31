﻿// <auto-generated />
using System;
using DigitalyAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DigitalyAPI.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20241219175426_dbrecuserr")]
    partial class dbrecuserr
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DigitalyAPI.Models.Domain.Client", b =>
                {
                    b.Property<int>("IdClient")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdClient"), 1L, 1);

                    b.Property<string>("Anciennete")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Datedenaiss")
                        .HasColumnType("datetime2");

                    b.Property<string>("Fonction")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nationnalite")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PortefeuilleId")
                        .HasColumnType("int");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RecouvreurId")
                        .HasColumnType("int");

                    b.Property<string>("adresse")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("profession")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("refclient")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("situationfamiliale")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("telephone")
                        .HasColumnType("int");

                    b.HasKey("IdClient");

                    b.HasIndex("PortefeuilleId");

                    b.HasIndex("RecouvreurId");

                    b.ToTable("clients");
                });

            modelBuilder.Entity("DigitalyAPI.Models.Domain.CompteBancaire", b =>
                {
                    b.Property<int>("Idcompte")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Idcompte"), 1L, 1);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("classe_risque")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("devisecompte")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("engagement_total")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("iban")
                        .HasColumnType("bigint");

                    b.Property<string>("mnt_bloque")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("rib")
                        .HasColumnType("bigint");

                    b.Property<string>("salairemensuel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("segmentation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("soldedispo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("tot_mvt_cred")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Idcompte");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("comptesBancaires");
                });

            modelBuilder.Entity("DigitalyAPI.Models.Domain.Impaye", b =>
                {
                    b.Property<int>("Idimpaye")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Idimpaye"), 1L, 1);

                    b.Property<int?>("ClientId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("PrestataireId")
                        .HasColumnType("int");

                    b.Property<DateTime>("date_impaye")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("echeance_Principale")
                        .HasColumnType("datetime2");

                    b.Property<string>("interet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("mt_encours")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("mt_impaye")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("mt_total")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ref_impaye")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("retard")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("statut")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("type_credit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("typeimpaye")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Idimpaye");

                    b.HasIndex("ClientId");

                    b.HasIndex("PrestataireId");

                    b.ToTable("impayes");
                });

            modelBuilder.Entity("DigitalyAPI.Models.Domain.Portefeuille", b =>
                {
                    b.Property<int>("IdPortefeuille")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPortefeuille"), 1L, 1);

                    b.Property<DateTime>("DateCreation")
                        .HasColumnType("datetime2");

                    b.Property<int>("nbreDossiers")
                        .HasColumnType("int");

                    b.Property<string>("nomPortefeuille")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("typeportefeuille")
                        .HasColumnType("int");

                    b.HasKey("IdPortefeuille");

                    b.ToTable("portefeuilles");
                });

            modelBuilder.Entity("DigitalyAPI.Models.Domain.Prestataire", b =>
                {
                    b.Property<int>("IdPrestataire")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPrestataire"), 1L, 1);

                    b.Property<string>("Profile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("adresse")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("agence")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("anciennete")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("civilite")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("codepostal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fax")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("mobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("nombre_dossiers_encharge")
                        .HasColumnType("int");

                    b.Property<string>("pays")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("prenom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("refprestataire")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("role")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ville")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("zone_geo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdPrestataire");

                    b.ToTable("prestataires");
                });

            modelBuilder.Entity("DigitalyAPI.Models.Domain.Recouvreur", b =>
                {
                    b.Property<int>("IdRecouvreur")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRecouvreur"), 1L, 1);

                    b.Property<int?>("PortefeuilleId")
                        .HasColumnType("int");

                    b.Property<string>("Profile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("adresse")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("anciennete")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("civilite")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("codepostal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fax")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fonction")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("grade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("mobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pays")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("prenom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("refrecouvreur")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ville")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdRecouvreur");

                    b.HasIndex("PortefeuilleId");

                    b.HasIndex("UserId");

                    b.ToTable("recouvreurs");
                });

            modelBuilder.Entity("DigitalyAPI.Models.Domain.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Civilite")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Dateofbirth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Fax")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Grade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneFix")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("longInPosition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "a1b2ab9f-bfde-4196-9350-e5ca4a3d143f",
                            ConcurrencyStamp = "1",
                            Name = "Admin",
                            NormalizedName = "Admin"
                        },
                        new
                        {
                            Id = "de25d4cf-ee7b-4db7-8a4d-630c93f347bf",
                            ConcurrencyStamp = "2",
                            Name = "RecouvreurAimable",
                            NormalizedName = "RecouvreurAimable"
                        },
                        new
                        {
                            Id = "d29c5d25-3d5e-4a7f-b590-6081a0350630",
                            ConcurrencyStamp = "3",
                            Name = "RecouvreurContentieux",
                            NormalizedName = "RecouvreurContentieux"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DigitalyAPI.Models.Domain.Client", b =>
                {
                    b.HasOne("DigitalyAPI.Models.Domain.Portefeuille", "Portefeuille")
                        .WithMany("Clients")
                        .HasForeignKey("PortefeuilleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DigitalyAPI.Models.Domain.Recouvreur", "Recouvreur")
                        .WithMany("Clients")
                        .HasForeignKey("RecouvreurId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Portefeuille");

                    b.Navigation("Recouvreur");
                });

            modelBuilder.Entity("DigitalyAPI.Models.Domain.CompteBancaire", b =>
                {
                    b.HasOne("DigitalyAPI.Models.Domain.Client", "Client")
                        .WithOne("compteBancaire")
                        .HasForeignKey("DigitalyAPI.Models.Domain.CompteBancaire", "ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("DigitalyAPI.Models.Domain.Impaye", b =>
                {
                    b.HasOne("DigitalyAPI.Models.Domain.Client", "Client")
                        .WithMany("impayes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalyAPI.Models.Domain.Prestataire", "Prestataire")
                        .WithMany("Impayes")
                        .HasForeignKey("PrestataireId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Client");

                    b.Navigation("Prestataire");
                });

            modelBuilder.Entity("DigitalyAPI.Models.Domain.Recouvreur", b =>
                {
                    b.HasOne("DigitalyAPI.Models.Domain.Portefeuille", "Portefeuille")
                        .WithMany("Recouvreurs")
                        .HasForeignKey("PortefeuilleId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("DigitalyAPI.Models.Domain.User", "user")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Portefeuille");

                    b.Navigation("user");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DigitalyAPI.Models.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DigitalyAPI.Models.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalyAPI.Models.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DigitalyAPI.Models.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DigitalyAPI.Models.Domain.Client", b =>
                {
                    b.Navigation("compteBancaire");

                    b.Navigation("impayes");
                });

            modelBuilder.Entity("DigitalyAPI.Models.Domain.Portefeuille", b =>
                {
                    b.Navigation("Clients");

                    b.Navigation("Recouvreurs");
                });

            modelBuilder.Entity("DigitalyAPI.Models.Domain.Prestataire", b =>
                {
                    b.Navigation("Impayes");
                });

            modelBuilder.Entity("DigitalyAPI.Models.Domain.Recouvreur", b =>
                {
                    b.Navigation("Clients");
                });
#pragma warning restore 612, 618
        }
    }
}
