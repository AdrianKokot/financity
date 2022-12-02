using System.Collections.Immutable;
using System.Security.Claims;
using Financity.Application.Abstractions.Data;
using Financity.Domain.Entities;
using Financity.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Financity.Presentation.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContext;
    private ImmutableHashSet<Guid>? _userWalletIds;

    private IImmutableDictionary<Guid, WalletAccessLevel>? _userWallets;

    public CurrentUserService(IHttpContextAccessor httpContext, IOptions<IdentityOptions> identityOptions)
    {
        _httpContext = httpContext;
        var claimsOptions = identityOptions.Value.ClaimsIdentity;
        var userId = httpContext.HttpContext?.User.FindFirstValue(claimsOptions.UserIdClaimType);

        if (string.IsNullOrEmpty(userId)) return;

        NormalizedUserEmail = httpContext.HttpContext?.User.FindFirstValue(claimsOptions.EmailClaimType)?.ToUpper() ??
                              string.Empty;

        IsAuthenticated = true;
        UserId = Guid.Parse(userId);
    }

    private IApplicationDbContext? DbContext =>
        _httpContext.HttpContext?.RequestServices.GetService<IApplicationDbContext>();

    public bool IsAuthenticated { get; }
    public Guid UserId { get; } = Guid.Empty;
    public string NormalizedUserEmail { get; } = string.Empty;


    public IImmutableDictionary<Guid, WalletAccessLevel> UserWallets =>
        (_userWallets ??= DbContext?.GetDbSet<Wallet>()
                                   .Where(x => x.OwnerId == UserId || x.UsersWithSharedAccess.Any(y => y.Id == UserId))
                                   .ToImmutableDictionary(x => x.Id,
                                       x => x.OwnerId == UserId
                                           ? WalletAccessLevel.Owner
                                           : WalletAccessLevel.Shared)) ??
        ImmutableDictionary<Guid, WalletAccessLevel>.Empty;

    public ImmutableHashSet<Guid> UserWalletIds => _userWalletIds ??= UserWallets.Keys.ToImmutableHashSet();
}