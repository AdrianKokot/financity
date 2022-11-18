using System.Collections.Immutable;
using Financity.Application.Abstractions.Data;
using Financity.Domain.Common;
using Financity.Domain.Entities;
using Financity.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

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
        return ruleBuilder.MustAsync(dbContext.Exists<TEntity>)
                          .WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");
    }


    public static IRuleBuilder<T, Guid> ExistsForCurrentUser<T, TEntity>(this IRuleBuilder<T, Guid> ruleBuilder,
                                                                         IApplicationDbContext dbContext)
        where TEntity : class, IEntity, IBelongsToWallet
    {
        return ruleBuilder
               .MustAsync(dbContext.ExistsForCurrentUser<TEntity>)
               .WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");
    }

    public static IRuleBuilder<T, IEnumerable<Guid>> ExistsForCurrentUser<T, TEntity>(
        this IRuleBuilder<T, IEnumerable<Guid>> ruleBuilder,
        IApplicationDbContext dbContext)
        where TEntity : class, IEntity, IBelongsToWallet
    {
        ruleBuilder.MustAsync(async (ids, ct) =>
                       await dbContext.ExistForCurrentUser<TEntity>(ids.ToImmutableHashSet(), ct))
                   .WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");

        return ruleBuilder;
    }

    public static IRuleBuilder<T, Guid> HasUserAccessToWallet<T>(this IRuleBuilder<T, Guid> builder,
                                                                 IApplicationDbContext db)
    {
        return builder.Must(id => db.UserService.UserWallets.ContainsKey(id))
                      .WithMessage($"{nameof(Wallet)} with given id doesn't exist.");
    }

    public static IRuleBuilder<T, Guid> HasUserAccessToWallet<T>(this IRuleBuilder<T, Guid> builder,
                                                                 IApplicationDbContext db,
                                                                 WalletAccessLevel accessLevel)
    {
        return builder.Must(id => db.UserService.UserWallets.TryGetValue(id, out var value) && value == accessLevel)
                      .WithMessage($"You cannot perform this operation on {nameof(Wallet)} with given id.");
    }

    public static IRuleBuilder<T, Guid?> ExistsIfNotNull<T, TEntity>(this IRuleBuilder<T, Guid?> builder,
                                                                     IApplicationDbContext db)
        where TEntity : class, IEntity
    {
        return builder.MustAsync(async (id, ct) =>
                          id is null || await db.Exists<TEntity>((Guid)id, ct))
                      .WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");
    }

    public static IRuleBuilder<T, Guid?> ExistsIfNotNullForCurrentUser<T, TEntity>(
        this IRuleBuilder<T, Guid?> builder, IApplicationDbContext db)
        where TEntity : class, IEntity, IBelongsToWallet
    {
        return builder.MustAsync(async (id, ct) => id is null || await db.ExistsForCurrentUser<TEntity>((Guid)id, ct))
                      .WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");
    }
}