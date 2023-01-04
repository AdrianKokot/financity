using System.Globalization;
using System.Reflection;
using EntityFramework.Exceptions.PostgreSQL;
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
        return FromExpression(() =>
            SearchUserTransactions(userId, searchTerm.ToLower(CultureInfo.InvariantCulture), walletId));
    }

    public async Task<int> GenerateDefaultCategories(Guid walletId, CancellationToken ct)
    {
        return await Database.ExecuteSqlRawAsync("CALL \"GenerateDefaultCategories\"({0})", walletId);
    }

    public DbSet<T> GetDbSet<T>() where T : class
    {
        return Set<T>();
    }

    public Task<int> DeleteWalletAsync(Guid id, CancellationToken ct)
    {
        return Database.ExecuteSqlRawAsync(
            "ALTER TABLE \"Transactions\" DISABLE TRIGGER ALL; DELETE FROM \"Transactions\" WHERE \"WalletId\" = {0}; ALTER TABLE \"Transactions\" ENABLE TRIGGER ALL; " +
            "ALTER TABLE \"Labels\" DISABLE TRIGGER ALL; DELETE FROM \"Labels\" WHERE \"WalletId\" = {0}; ALTER TABLE \"Labels\" ENABLE TRIGGER ALL; " +
            "ALTER TABLE \"Recipients\" DISABLE TRIGGER ALL; DELETE FROM \"Recipients\" WHERE \"WalletId\" = {0}; ALTER TABLE \"Recipients\" ENABLE TRIGGER ALL; " +
            "ALTER TABLE \"Categories\" DISABLE TRIGGER ALL; DELETE FROM \"Categories\" WHERE \"WalletId\" = {0}; ALTER TABLE \"Categories\" ENABLE TRIGGER ALL; " +
            "DELETE FROM \"Wallets\" WHERE \"Id\" = {0};", id);
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.SeedData();

        builder.HasDbFunction(
            GetType().GetMethod(nameof(SearchUserTransactions), new[] { typeof(Guid), typeof(string), typeof(Guid?) })!
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