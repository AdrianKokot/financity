using System.Collections.Immutable;

namespace Financity.Application.Abstractions.Data;

public interface ICurrentUserService
{
    public bool IsAuthenticated { get; }
    public Guid UserId { get; }
    public ImmutableHashSet<Guid> UserWallets { get; }
}