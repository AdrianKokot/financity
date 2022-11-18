using System.Collections.Immutable;
using Financity.Domain.Enums;

namespace Financity.Application.Abstractions.Data;

public interface ICurrentUserService
{
    public bool IsAuthenticated { get; }
    public Guid UserId { get; }
    public string NormalizedUserEmail { get; }
    public IImmutableDictionary<Guid, WalletAccessLevel> UserWallets { get; }
    public ImmutableHashSet<Guid> UserWalletIds { get; }
}