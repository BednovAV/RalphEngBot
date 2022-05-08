using Entities;
using Entities.Common;
using Entities.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer.Core
{
    public class ApplicationContext : DbContext
    {
        private readonly string _connectionString;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<WordTranslation> WordTranslations { get; set; } = null!;
        public DbSet<GrammarTest> GrammarTests { get; set; } = null!;
        public DbSet<TestQuestion> TestQuestions { get; set; } = null!;
        public DbSet<UserTest> UserTests { get; set; } = null!;
        public DbSet<UserQuestion> UserQuestions { get; set; } = null!;
        public ApplicationContext(string connectionString)
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasMany(u => u.WordTranslations)
                .WithMany(w => w.Users)
                .UsingEntity<UserWord>(
                   j => j
                    .HasOne(pt => pt.WordTranslation)
                    .WithMany(p => p.UserWords)
                    .HasForeignKey(pt => pt.WordTranslationId),
                   j => j
                    .HasOne(pt => pt.User)
                    .WithMany(t => t.UserWords)
                    .HasForeignKey(pt => pt.UserId),
                   j =>
                    {
                        j.Property(pt => pt.Status).HasDefaultValue(WordStatus.NotSelected);
                        j.Property(pt => pt.Recognitions).HasDefaultValue(0);
                        j.Property(pt => pt.Order).HasDefaultValue(null);
                        j.HasKey(t => new { t.UserId, t.WordTranslationId });
                        j.ToTable("UserWords");
                    });
            modelBuilder.Entity<UserTest>().HasKey(ut => new { ut.UserId, ut.GrammarTestId });
        }
    }
}
