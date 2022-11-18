using System.Collections.Immutable;
using Financity.Application.Abstractions.Data;
using Financity.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Common.Extensions;

public static class ApplicationDbContextExtensions
{
    internal static Task<bool> Exists<TEntity>(this IApplicationDbContext ctx, Guid id, CancellationToken ct)
        where TEntity : class, IEntity
    {
        return ctx.GetDbSet<TEntity>()
                  .AsNoTracking()
                  .AnyAsync(x => x.Id == id, ct);
    }

    public static Task<bool> HasUserAccess<TEntity>(this IApplicationDbContext ctx,
                                                    ImmutableHashSet<Guid> ids,
                                                    CancellationToken ct)
        where TEntity : class, IEntity, IBelongsToWallet
    {
        return ctx.GetDbSet<TEntity>()
                  .AsNoTracking()
                  .Where(x => ids.Contains(x.Id))
                  .AllAsync(x => ctx.UserService.UserWalletIds.Contains(x.WalletId), ct);
    }

    public static Task<bool> HasUserAccess<TEntity>(this IApplicationDbContext ctx,
                                                    Guid id,
                                                    CancellationToken ct)
        where TEntity : class, IEntity, IBelongsToWallet
    {
        return ctx.GetDbSet<TEntity>()
                  .AsNoTracking()
                  .AnyAsync(x => x.Id == id && ctx.UserService.UserWalletIds.Contains(x.WalletId), ct);
    }
}