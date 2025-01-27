using System.Data.Entity;
using EF6Demo.Models;

namespace EF6Demo.Data
{
    public class SalesContext : DbContext
    {
        public SalesContext() : base("name=SalesConnection")
        {
            // Enable automatic migrations
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SalesContext, Migrations.Configuration>());
        }

        public DbSet<SalesTicket> SalesTickets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure SalesTicket entity
            modelBuilder.Entity<SalesTicket>()
                .Property(s => s.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesTicket>()
                .Property(s => s.DiscountAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesTicket>()
                .Property(s => s.TaxAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SalesTicket>()
                .Property(s => s.TicketNumber)
                .IsRequired()
                .HasMaxLength(20);

            // Create unique index for TicketNumber
            modelBuilder.Entity<SalesTicket>()
                .HasIndex(s => s.TicketNumber)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
} 