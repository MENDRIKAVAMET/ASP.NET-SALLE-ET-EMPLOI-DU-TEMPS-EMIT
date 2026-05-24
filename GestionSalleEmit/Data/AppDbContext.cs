using Microsoft.EntityFrameworkCore;
using GestionSalleEmit.Models;

namespace GestionSalleEmit.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Filiere> Filieres { get; set; }
        public DbSet<Parcours> Parcours { get; set; }
        public DbSet<Niveau> Niveaux { get; set; }
        public DbSet<Enseignant> Enseignants { get; set; }
        public DbSet<Matiere> Matieres { get; set; }
        public DbSet<EmploiDuTemps> EmploisDuTemps { get; set; }
        public DbSet<Salle> Salles { get; set; }
        public DbSet<Enseigner> Enseigners { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // UTILISATEUR
            // =========================
            modelBuilder.Entity<Utilisateur>()
                .HasKey(u => u.IdUtilisateur);

            // =========================
            // FILIERE
            // =========================
            modelBuilder.Entity<Filiere>()
                .HasKey(f => f.IdFiliere);

            modelBuilder.Entity<Filiere>()
                .Property(f => f.NomFiliere)
                .IsRequired()
                .HasMaxLength(100);

            // =========================
            // PARCOURS
            // =========================
            modelBuilder.Entity<Parcours>()
                .HasKey(p => p.IdParcours);

            modelBuilder.Entity<Parcours>()
                .Property(p => p.NomParcours)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Parcours>()
                .HasOne(p => p.Filiere)
                .WithMany(f => f.Parcours)
                .HasForeignKey(p => p.IdFiliere)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // NIVEAU
            // =========================
            modelBuilder.Entity<Niveau>()
                .HasKey(n => n.IdNiveau);

            modelBuilder.Entity<Niveau>()
                .Property(n => n.NomNiveau)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Niveau>()
                .HasOne(n => n.Parcours)
                .WithMany(p => p.Niveaux)
                .HasForeignKey(n => n.IdParcours)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // SALLE
            // =========================
            modelBuilder.Entity<Salle>()
                .HasKey(s => s.IdSalle);

            // =========================
            // MATIERE
            // =========================
            modelBuilder.Entity<Matiere>()
                .HasKey(m => m.IdMatiere);

            modelBuilder.Entity<Matiere>()
                .Property(m => m.NomMatiere)
                .IsRequired()
                .HasMaxLength(100);

            // =========================
            // ENSEIGNANT
            // =========================
            modelBuilder.Entity<Enseignant>()
                .HasKey(e => e.IdEnseignant);

            // =========================
            // ENSEIGNER (Many-to-Many)
            // =========================
            modelBuilder.Entity<Enseigner>()
                .HasKey(e => new
                {
                    e.IdEnseignant,
                    e.IdMatiere
                });

            modelBuilder.Entity<Enseigner>()
                .HasOne(e => e.Enseignant)
                .WithMany(en => en.Enseigners)
                .HasForeignKey(e => e.IdEnseignant)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enseigner>()
                .HasOne(e => e.Matiere)
                .WithMany(m => m.Enseigners)
                .HasForeignKey(e => e.IdMatiere)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // EMPLOI DU TEMPS
            // =========================
            modelBuilder.Entity<EmploiDuTemps>()
                .HasKey(e => e.IdEDT);

            modelBuilder.Entity<EmploiDuTemps>()
                .Property(e => e.Jour)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<EmploiDuTemps>()
                .Property(e => e.Semestre)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<EmploiDuTemps>()
                .HasOne(e => e.Salle)
                .WithMany()
                .HasForeignKey(e => e.IdSalle)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmploiDuTemps>()
                .HasOne(e => e.Enseignant)
                .WithMany(en => en.EmploisDuTemps)
                .HasForeignKey(e => e.IdEnseignant)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmploiDuTemps>()
                .HasOne(e => e.Matiere)
                .WithMany(m => m.EmploisDuTemps)
                .HasForeignKey(e => e.IdMatiere)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmploiDuTemps>()
                .HasOne(e => e.Niveau)
                .WithMany(n => n.EmploisDuTemps)
                .HasForeignKey(e => e.IdNiveau)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}