using System.Reflection;
using Financity.Application.Abstractions.Data;
using Financity.Domain.Common;
using Financity.Domain.Entities;
using Financity.Persistence.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Financity.Persistence.Database;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IApplicationDbContext
{
    private readonly ICurrentUserService? _userService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService userService)
        : base(options)
    {
        _userService = userService;
    }

    public DbSet<T> GetDbSet<T>() where T : class
    {
        return Set<T>();
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Recipient> Recipients { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Wallet> Wallets { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = AppDateTime.Now;
                    entry.Entity.CreatedBy = _userService?.UserId;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = AppDateTime.Now;
                    entry.Entity.UpdatedBy = _userService?.UserId;
                    break;

                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                default:
                    break;
            }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.SeedData();

        base.OnModelCreating(modelBuilder);

        CustomizeIdentity(modelBuilder);
    }

    private void CustomizeIdentity(ModelBuilder builder)
    {
        builder.Entity<User>(b =>
        {
            b.ToTable("Users");
            b.Ignore(u => u.PhoneNumber);
            b.Ignore(u => u.PhoneNumberConfirmed);
        });

        builder.Ignore<IdentityRole<Guid>>();
        builder.Ignore<IdentityRoleClaim<Guid>>();
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Ignore<IdentityUserToken<Guid>>();
        builder.Ignore<IdentityUserLogin<Guid>>();
        builder.Ignore<IdentityUserRole<Guid>>();
    }
}