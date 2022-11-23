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

    public ICurrentUserService UserService =>
        _userService ?? throw new ArgumentNullException(nameof(ICurrentUserService));

    public IQueryable<Transaction> SearchUserTransactions(Guid userId, string searchTerm, Guid? walletId = null)
    {
        return FromExpression(() => SearchUserTransactions(userId, searchTerm, walletId));
    }

    public DbSet<T> GetDbSet<T>() where T : class
    {
        return Set<T>();
    }

    public Task<int> DeleteFromSetAsync<T>(Guid entityId, CancellationToken ct) where T : class, IEntity
    {
        return DeleteFromSetAsync(Set<T>().Where(x => x.Id == entityId), ct);
    }

    public Task<int> DeleteFromSetAsync<T>(IQueryable<T> query, CancellationToken ct)
        where T : class, IEntity
    {
        return query.ExecuteDeleteAsync(ct);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.SeedData();

        builder.HasDbFunction(
            GetType().GetMethod(nameof(SearchUserTransactions), new[] {typeof(Guid), typeof(string), typeof(Guid?)})!
        );

        base.OnModelCreating(builder);

        CustomizeIdentity(builder);
    }

    private static void CustomizeIdentity(ModelBuilder builder)
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