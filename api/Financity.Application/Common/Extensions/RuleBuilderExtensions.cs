using System.Collections.Immutable;
using Financity.Application.Abstractions.Data;
using Financity.Domain.Common;
using Financity.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Common.Extensions;

public static class RuleBuilderExtensions
{
    public static IRuleBuilder<T, string> MatchesIf<T>(this IRuleBuilder<T, string> ruleBuilder,
                                                       string matches, bool shouldMatch)
    {
        var result = ruleBuilder;

        if (shouldMatch) result = result.Matches(matches);

        return result;
    }

    public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, PasswordOptions options)
    {
        return ruleBuilder.NotEmpty()
                          .MatchesIf("[A-Z]", options.RequireUppercase)
                          .MatchesIf("[a-z]", options.RequireLowercase)
                          .MatchesIf("[0-9]", options.RequireDigit)
                          .MatchesIf("[^a-zA-Z0-9]", options.RequireNonAlphanumeric)
                          .MinimumLength(options.RequiredLength)
                          .MaximumLength(int.MaxValue);
    }

    public static IRuleBuilder<T, Guid> Exists<T, TEntity>(this IRuleBuilder<T, Guid> ruleBuilder,
                                                           IApplicationDbContext dbContext)
        where TEntity : class, IEntity
    {
        ruleBuilder.MustAsync(dbContext.Exists<TEntity>)
                   .WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");

        return ruleBuilder;
    }

    private static Task<bool> Exists<TEntity>(this IApplicationDbContext ctx, Guid id, CancellationToken ct)
        where TEntity : class, IEntity
    {
        return ctx.GetDbSet<TEntity>().AsNoTracking().AnyAsync(x => x.Id == id, ct);
    }

    public static IRuleBuilder<T, Guid> ExistsForCurrentUser<T, TEntity>(this IRuleBuilder<T, Guid> ruleBuilder,
                                                                         IApplicationDbContext dbContext)
        where TEntity : class, IEntity, IBelongsToWallet
    {
        return ruleBuilder
               .MustAsync(dbContext.ExistsForCurrentUser<TEntity>)
               .WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");
    }

    private static Task<bool> ExistsForCurrentUser<TEntity>(this IApplicationDbContext dbContext,
                                                            ImmutableHashSet<Guid> ids,
                                                            CancellationToken cancellation)
        where TEntity : class, IEntity, IBelongsToWallet
    {
        return dbContext.GetDbSet<TEntity>().AsNoTracking()
                        .Where(x => ids.Contains(x.Id))
                        .AllAsync(x => dbContext.UserService.UserWallets.Contains(x.WalletId), cancellation);
    }

    private static Task<bool> ExistsForCurrentUser<TEntity>(this IApplicationDbContext dbContext, Guid id,
                                                            CancellationToken cancellation)
        where TEntity : class, IEntity, IBelongsToWallet
    {
        return dbContext.GetDbSet<TEntity>().AsNoTracking()
                        .AnyAsync(
                            x => x.Id == id && dbContext.UserService.UserWallets.Contains(x.WalletId),
                            cancellation);
    }

    public static IRuleBuilder<T, IEnumerable<Guid>> ExistsForCurrentUser<T, TEntity>(
        this IRuleBuilder<T, IEnumerable<Guid>> ruleBuilder,
        IApplicationDbContext dbContext)
        where TEntity : class, IEntity, IBelongsToWallet
    {
        ruleBuilder.MustAsync(async (enumerable, cancellation) =>
        {
            var ids = enumerable.ToImmutableHashSet();
            return await dbContext.GetDbSet<TEntity>().AsNoTracking()
                                  .Where(x => ids.Contains(x.Id))
                                  .AllAsync(x => dbContext.UserService.UserWallets.Contains(x.WalletId), cancellation);
        }).WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");

        return ruleBuilder;
    }

    public static IRuleBuilder<T, Guid> HasUserAccessToWallet<T>(this IRuleBuilder<T, Guid> ruleBuilder,
                                                                 IApplicationDbContext dbContext)
    {
        return ruleBuilder.Must(id => dbContext.UserService.UserWallets.Contains(id))
                          .WithMessage($"{nameof(Wallet)} with given id doesn't exist.");
    }

    public static IRuleBuilder<T, Guid?> ExistsIfNotNull<T, TEntity>(this IRuleBuilder<T, Guid?> ruleBuilder,
                                                                     IApplicationDbContext dbContext)
        where TEntity : class, IEntity
    {
        ruleBuilder.MustAsync(async (entityId, cancellation) =>
        {
            return entityId is null || await dbContext.GetDbSet<TEntity>().AsNoTracking()
                                                      .AnyAsync(x => x.Id == entityId, cancellation);
        }).WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");

        return ruleBuilder;
    }

    public static IRuleBuilder<T, Guid?> ExistsIfNotNullForCurrentUser<T, TEntity>(
        this IRuleBuilder<T, Guid?> ruleBuilder,
        IApplicationDbContext dbContext)
        where TEntity : class, IEntity, IBelongsToWallet
    {
        ruleBuilder.MustAsync(async (entityId, cancellation) =>
        {
            return entityId is null || await dbContext.GetDbSet<TEntity>().AsNoTracking()
                                                      .AnyAsync(x => x.Id == entityId, cancellation);
        }).WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");

        return ruleBuilder;
    }
}