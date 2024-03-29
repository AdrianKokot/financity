﻿using System.Collections.Immutable;
using Financity.Application.Abstractions.Data;
using Financity.Domain.Common;
using Financity.Domain.Entities;
using Financity.Domain.Enums;
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
        return ruleBuilder.MustAsync(dbContext.Exists<TEntity>)
                          .WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");
    }

    public static IRuleBuilder<T, TKey> Exists<T, TEntity, TKey>(
        this IRuleBuilder<T, TKey> ruleBuilder,
        IApplicationDbContext dbContext)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return ruleBuilder.MustAsync(dbContext.Exists<TEntity, TKey>)
                          .WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");
    }

    public static IRuleBuilder<T, Guid> HasUserAccess<T, TEntity>(this IRuleBuilder<T, Guid> ruleBuilder,
                                                                  IApplicationDbContext dbContext)
        where TEntity : class, IEntity, IBelongsToWallet
    {
        return ruleBuilder
               .MustAsync(dbContext.HasUserAccess<TEntity>)
               .WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");
    }

    public static IRuleBuilder<T, Guid> UserOwns<T, TEntity>(this IRuleBuilder<T, Guid> ruleBuilder,
                                                             IApplicationDbContext dbContext)
        where TEntity : class, IEntity, IBelongsToUser
    {
        return ruleBuilder
               .MustAsync(async (id, ct) => await dbContext.GetDbSet<TEntity>()
                                                           .AsNoTracking()
                                                           .AnyAsync(
                                                               x => x.Id == id &&
                                                                    x.UserId == dbContext.UserService.UserId, ct))
               .WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");
    }

    public static IRuleBuilder<T, IEnumerable<Guid>> HasUserAccess<T, TEntity>(
        this IRuleBuilder<T, IEnumerable<Guid>> ruleBuilder,
        IApplicationDbContext dbContext)
        where TEntity : class, IEntity, IBelongsToWallet
    {
        ruleBuilder.MustAsync(async (ids, ct) =>
                       await dbContext.HasUserAccess<TEntity>(ids.ToImmutableHashSet(), ct))
                   .WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");

        return ruleBuilder;
    }

    public static IRuleBuilder<T, Guid> HasUserAccessToWallet<T>(this IRuleBuilder<T, Guid> builder,
                                                                 IApplicationDbContext dbContext)
    {
        return builder.Must(dbContext.UserService.UserWallets.ContainsKey)
                      .WithMessage($"{nameof(Wallet)} with given id doesn't exist.");
    }

    public static IRuleBuilder<T, Guid> HasUserAccessToWallet<T>(this IRuleBuilder<T, Guid> builder,
                                                                 IApplicationDbContext dbContext,
                                                                 WalletAccessLevel accessLevel)
    {
        return builder
               .Must(id => dbContext.UserService.UserWallets.TryGetValue(id, out var value) && value == accessLevel)
               .WithMessage($"You cannot perform this operation on {nameof(Wallet)} with given id.");
    }

    public static IRuleBuilder<T, Guid?> ExistsOrNull<T, TEntity>(this IRuleBuilder<T, Guid?> builder,
                                                                  IApplicationDbContext db)
        where TEntity : class, IEntity
    {
        return builder.MustAsync(async (id, ct) =>
                          id is null || await db.Exists<TEntity>((Guid)id, ct))
                      .WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");
    }

    public static IRuleBuilder<T, Guid?> HasUserAccessOrNull<T, TEntity>(
        this IRuleBuilder<T, Guid?> builder, IApplicationDbContext dbContext)
        where TEntity : class, IEntity, IBelongsToWallet
    {
        return builder.MustAsync(async (id, ct) => id is null || await dbContext.HasUserAccess<TEntity>((Guid)id, ct))
                      .WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");
    }
}