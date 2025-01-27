using System.Data.Entity;
using EF6Demo.Models;

namespace EF6Demo.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext() : base("name=SchoolConnection")
        {
            // Enable automatic migrations
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SchoolContext, Migrations.Configuration>());
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure Student entity
            modelBuilder.Entity<Student>()
                .Property(s => s.GPA)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Student>()
                .Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);

            base.OnModelCreating(modelBuilder);
        }
    }
} 