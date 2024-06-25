using APBD_PROJEKT.model;
using Microsoft.EntityFrameworkCore;

namespace APBD_PROJEKT.database;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<Contract> Contracts { get; set; }

    public DbSet<Subscription> Subscriptions { get; set; }

    public DbSet<Discount> Discounts { get; set; }

    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IndividualClient>().ToTable("IndividualClients");
        modelBuilder.Entity<CompanyClient>().ToTable("CompanyClients");
    }
}