using ClientServiceRazor.Features.Clients.Models;
using ClientServiceRazor.Features.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientServiceRazor.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<FinanceAccount> FinanceAccounts { get; set; }
    public DbSet<ClientFinanceAccount> ClientFinanceAccounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Client>()
            .HasIndex(b => b.Email)
            .IsUnique();
        //Створення складеного первинного ключа для ClientFinanceAccount
        modelBuilder.Entity<ClientFinanceAccount>()
            .HasKey(cfa => new { cfa.ClientId, cfa.FinanceAccountId });
    }
}