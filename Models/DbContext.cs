using Microsoft.EntityFrameworkCore;
using QS.Models;

namespace QS.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<RepondantCategorie> RepondantCategories { get; set; }
        public DbSet<MedicalService> MedicalServices { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Repondant> Repondants { get; set; }
        public DbSet<Reponse> Reponses { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<FormResultEntity> FormResultEntities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Repondant>()
                .HasOne(r => r.Categorie)
                .WithMany(c => c.Repondants)
                .HasForeignKey(r => r.CategorieId)
                .OnDelete(DeleteBehavior.SetNull); // Ajout de l'onDelete si nécessaire

            modelBuilder.Entity<Repondant>()
                .HasOne(r => r.Service)
                .WithMany() // ou .WithMany(s => s.Repondants) si tu veux la navigation inverse
                .HasForeignKey(r => r.ServiceId)
                .OnDelete(DeleteBehavior.SetNull); // Ajouter comportement delete si nécessaire

            modelBuilder.Entity<Reponse>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Reponses)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade); // Définir comportement delete en cascade si nécessaire

            modelBuilder.Entity<Reponse>()
                .HasOne(r => r.Repondant)
                .WithMany(r => r.Reponses)
                .HasForeignKey(r => r.RepondantId)
                .OnDelete(DeleteBehavior.Cascade); // Définir comportement delete en cascade si nécessaire

            modelBuilder.Entity<MedicalService>()
                .HasIndex(ms => ms.Code)
                .IsUnique(); // Assurer l'unicité du Code

            base.OnModelCreating(modelBuilder);
        }

        // Si vous avez une méthode pour initialiser des données de peuplement
        public static void SeedData(AppDbContext context)
        {
            if (!context.Questions.Any())
            {
                context.Questions.Add(new Question { Text = "Exemple de question" });
                context.SaveChanges();
            }
            
            // Ajoutez d'autres données si nécessaire pour les autres entités
        }
    }
}
