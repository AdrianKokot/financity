using System.Reflection;
using Financity.Application.Abstractions.Data;
using Financity.Domain.Entities;
using Financity.Domain.Shared;
using Financity.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace Financity.Persistence.Database;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Recipient> Recipients { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Wallet> Wallets { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.SeedData();
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = AppDateTime.Now;
                    entry.Entity.CreatedBy = string.Empty;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = AppDateTime.Now;
                    entry.Entity.UpdatedBy = string.Empty;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}