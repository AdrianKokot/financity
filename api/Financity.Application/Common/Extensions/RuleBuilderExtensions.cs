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
        ruleBuilder.MustAsync(async (entityId, cancellation) =>
        {
            return await dbContext.GetDbSet<TEntity>().AsNoTracking()
                                  .AnyAsync(x => x.Id == entityId, cancellation);
        }).WithMessage($"{typeof(TEntity).Name} with given id doesn't exist.");

        return ruleBuilder;
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
    
    public static IRuleBuilder<T, Guid> HasAccessToWallet<T>(this IRuleBuilder<T, Guid> ruleBuilder,
                                                             IApplicationDbContext dbContext,
                                                             ICurrentUserService userService)
    {
        ruleBuilder.MustAsync(async (walletId, ct) =>
        {
            return await dbContext.GetDbSet<WalletAccess>().AsNoTracking()
                                  .AnyAsync(a => a.UserId == userService.UserId && a.WalletId == walletId, ct);
        }).WithMessage($"{nameof(Wallet)} with given id doesn't exist.");

        return ruleBuilder;
    }
}