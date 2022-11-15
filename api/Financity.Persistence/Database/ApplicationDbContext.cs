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

    public DbSet<Budget> Budgets { get; set; }
    public DbSet<WalletAccess> WalletAccesses { get; set; }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Recipient> Recipients { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Wallet> Wallets { get; set; }

    public DbSet<T> GetDbSet<T>() where T : class
    {
        return Set<T>();
    }

    public Task<int> DeleteFromSetAsync<T>(Guid entityId, CancellationToken ct) where T : class, IEntity
    {
        return Set<T>().Where(x => x.Id == entityId).ExecuteDeleteAsync(ct);
    }

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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.SeedData();

        builder.Entity<Category>(entity => entity.OwnsOne(x => x.Appearance));
        builder.Entity<Label>(entity => entity.OwnsOne(x => x.Appearance));

        builder.Entity<WalletAccess>().HasKey(x => new {x.UserId, x.WalletId});

        base.OnModelCreating(builder);

        CustomizeIdentity(builder);
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