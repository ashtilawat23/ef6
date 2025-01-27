using System.Data.Entity;
using EF6Demo.Models;

namespace EF6Demo.Data
{
    public class BookContext : DbContext
    {
        public BookContext() : base("name=BookConnection")
        {
            // Enable automatic migrations
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BookContext, Migrations.Configuration>());
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure Book entity
            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Book>()
                .Property(b => b.Rating)
                .HasPrecision(2, 1);

            modelBuilder.Entity<Book>()
                .Property(b => b.ISBN)
                .IsRequired()
                .HasMaxLength(13);

            modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
} 