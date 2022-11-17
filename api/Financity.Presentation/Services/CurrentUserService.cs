using System.Collections.Immutable;
using System.Security.Claims;
using Financity.Application.Abstractions.Data;
using Financity.Domain.Common;

namespace Financity.Presentation.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContext;

    private ImmutableHashSet<Guid>? _userWallets;

    public CurrentUserService(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
        var userId = httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId)) return;

        IsAuthenticated = true;
        UserId = Guid.Parse(userId);
    }

    private IApplicationDbContext? DbContext =>
        _httpContext.HttpContext?.RequestServices.GetService<IApplicationDbContext>();

    public bool IsAuthenticated { get; }
    public Guid UserId { get; } = Guid.Empty;

    public ImmutableHashSet<Guid> UserWallets =>
        (_userWallets ??= DbContext?.GetDbSet<WalletAccess>()
                                   .Where(x => x.UserId == UserId)
                                   .Select(x => x.WalletId)
                                   .ToImmutableHashSet()) ?? ImmutableHashSet<Guid>.Empty;
}