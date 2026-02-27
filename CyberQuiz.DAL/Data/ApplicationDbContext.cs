using CyberQuiz.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.DAL
{
    // DbContext som ärver från IdentityDbContext
    // endast en db
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<UserResult> UserResults { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Anropa base för att få Identity-tabeller (användare, roller, etc.)
            base.OnModelCreating(modelBuilder);

            //Definiera relationer mellan tabeller

            // Category → SubCategory
            modelBuilder.Entity<Category>()
                .HasMany(c => c.SubCategories)
                .WithOne(sc => sc.Category)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // SubCategory → Question
            modelBuilder.Entity<SubCategory>()
                .HasMany(sc => sc.Questions)
                .WithOne(q => q.SubCategory)
                .HasForeignKey(q => q.SubCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Question → AnswerOption
            modelBuilder.Entity<Question>()
                .HasMany(q => q.AnswerOptions)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Question → UserResult
            modelBuilder.Entity<UserResult>()
                .HasOne(ur => ur.Question)
                .WithMany(q => q.UserResults)
                .HasForeignKey(ur => ur.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // AnswerOption → UserResult
            modelBuilder.Entity<UserResult>()
                .HasOne(ur => ur.AnswerOption)
                .WithMany(a => a.UserResults)
                .HasForeignKey(ur => ur.AnswerOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            // ApplicationUser → UserResult
            modelBuilder.Entity<UserResult>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserResults)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seeda initial data
            modelBuilder.SeedCyberQuizData();
        }
    }
}
