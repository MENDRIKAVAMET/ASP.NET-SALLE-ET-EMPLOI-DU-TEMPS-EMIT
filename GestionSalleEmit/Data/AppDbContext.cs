using Microsoft.EntityFrameworkCore;
using GestionSalleEmit.Models;

namespace GestionSalleEmit.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Filiere> Filieres { get; set; }
        public DbSet<Niveau> Niveaux { get; set; }
        public DbSet<Enseignant> Enseignants { get; set; }
        public DbSet<Matiere> Matieres { get; set; }
        public DbSet<EmploiDuTemps> EmploisDuTemps { get; set; }
        public DbSet<Salle> Salles { get; set; }
        public DbSet<Enseigner> Enseigners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Filiere
            modelBuilder.Entity<Filiere>()
                .HasKey(f => f.IdFiliere);

            // Configure Niveau
            modelBuilder.Entity<Niveau>()
                .HasKey(n => n.IdNiveau);
            modelBuilder.Entity<Niveau>()
                .HasOne(n => n.Filiere)
                .WithMany(f => f.Niveaux)
                .HasForeignKey(n => n.IdFiliere);

            // Configure Salle
            modelBuilder.Entity<Salle>()
                .HasKey(s => s.IdSalle);

            // Configure Matiere
            modelBuilder.Entity<Matiere>()
                .HasKey(m => m.IdMatiere);

            // Configure Enseignant
            modelBuilder.Entity<Enseignant>()
                .HasKey(e => e.IdEnseignant);

            // Configure Enseigner (clé composite)
            modelBuilder.Entity<Enseigner>()
                .HasKey(e => new { e.IdEnseignant, e.IdMatiere });
            modelBuilder.Entity<Enseigner>()
                .HasOne(e => e.Enseignant)
                .WithMany(en => en.Enseigners)
                .HasForeignKey(e => e.IdEnseignant);
            modelBuilder.Entity<Enseigner>()
                .HasOne(e => e.Matiere)
                .WithMany(m => m.Enseigners)
                .HasForeignKey(e => e.IdMatiere);

            // Configure EmploiDuTemps
            modelBuilder.Entity<EmploiDuTemps>()
                .HasKey(e => e.IdEDT);
            modelBuilder.Entity<EmploiDuTemps>()
                .HasOne(e => e.Salle)
                .WithMany()
                .HasForeignKey(e => e.IdSalle);
            modelBuilder.Entity<EmploiDuTemps>()
                .HasOne(e => e.Enseignant)
                .WithMany(en => en.EmploisDuTemps)
                .HasForeignKey(e => e.IdEnseignant);
            modelBuilder.Entity<EmploiDuTemps>()
                .HasOne(e => e.Matiere)
                .WithMany(m => m.EmploisDuTemps)
                .HasForeignKey(e => e.IdMatiere);
            modelBuilder.Entity<EmploiDuTemps>()
                .HasOne(e => e.Niveau)
                .WithMany(n => n.EmploisDuTemps)
                .HasForeignKey(e => e.IdNiveau);
        }
    }
}
