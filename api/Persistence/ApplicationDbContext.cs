using Financity.Domain.Common;
using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financity.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Entry> Entries { get; set; }
    public DbSet<EntryCategory> EntryCategories { get; set; }
    public DbSet<EntryType> EntryTypes { get; set; }
    public DbSet<Recipient> Recipients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Currency>(c =>
            {
                c.HasData(new Currency() {Id = 1, Code = "PLN", CreatedAt = DateTime.Now, CreatedBy = "System"});
            }
        );
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = string.Empty;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = string.Empty;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}